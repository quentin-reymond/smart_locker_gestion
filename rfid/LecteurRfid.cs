using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using uPLibrary.Networking.M2Mqtt; // Ajout de la bibliothèque MQTT
using uPLibrary.Networking.M2Mqtt.Messages; // Ajout de la bibliothèque MQTT

namespace GestionAbsence.RFID
{
    public class LecteurRfid
    {
        [DllImport("kernel32.dll")]
        static extern void Sleep(int dwMilliseconds);

        [DllImport("MasterRD.dll")]
        static extern int rf_init_com(int port, int baud);

        [DllImport("MasterRD.dll")]
        static extern int rf_ClosePort();

        [DllImport("MasterRD.dll")]
        static extern int rf_antenna_sta(short icdev, byte mode);

        [DllImport("MasterRD.dll")]
        static extern int rf_init_type(short icdev, byte type);

        [DllImport("MasterRD.dll")]
        static extern int rf_request(short icdev, byte mode, ref ushort pTagType);

        [DllImport("MasterRD.dll")]
        static extern int rf_anticoll(short icdev, byte bcnt, IntPtr pSnr, ref byte pRLength);

        public bool bConnectedDevice; // pour savoir si un lecteur est connecté
        protected int port;
        public int Port
        {
            get { return port; }
            set { if (value > 0) port = value; }
        }

        protected int baud;
        public int Baud
        {
            get { return baud; }
            set { if (value > 0) baud = value; }
        }

        protected string identifiant;
        public string Identifiant
        {
            get { return identifiant; }
            set { identifiant = value; }
        }

        private MqttClient mqttClient; // Client MQTT
        private string mqttBrokerAddress = "test.mosquitto.org"; // Remplacez par l'adresse IP de votre Raspberry Pi

        public LecteurRfid()
        {
            bConnectedDevice = false;
            baud = 19200;

            // Initialiser le client MQTT
            mqttClient = new MqttClient(mqttBrokerAddress);
            mqttClient.Connect(Guid.NewGuid().ToString());
        }

        public int connectionRs()
        {
            int status = rf_init_com(port, baud);
            bConnectedDevice = (status == 0);
            return status;
        }

        public int fermetureRs()
        {
            if (bConnectedDevice)
            {
                int status = rf_ClosePort();
                bConnectedDevice = (status == 0);
                return status;
            }
            return -1;
        }

        public int lireIdentifiantCarte()
        {
            int testlecture = 0;
            short icdev = 0x0000;
            int status = -1;
            byte type = (byte)'A';
            byte mode = 0x52;
            ushort TagType = 0;
            byte bcnt = 0x04;
            IntPtr pSnr = Marshal.AllocHGlobal(1024);
            byte len = 255;

            if (!bConnectedDevice)
            {
                connectionRs();
            }

            if (bConnectedDevice)
            {
                do
                {
                    status = rf_antenna_sta(icdev, 0);
                    if (status == 0)
                    {
                        Sleep(20);
                        status = rf_init_type(icdev, type);
                        if (status == 0)
                        {
                            Sleep(20);
                            status = rf_antenna_sta(icdev, 1);
                            if (status == 0)
                            {
                                Sleep(50);
                                status = rf_request(icdev, mode, ref TagType);
                                if (status == 0)
                                {
                                    status = rf_anticoll(icdev, bcnt, pSnr, ref len);
                                    if (status == 0)
                                    {
                                        byte[] szBytes = new byte[len];
                                        for (int j = 0; j < len; j++) szBytes[j] = Marshal.ReadByte(pSnr, j);
                                        string m_cardNo = String.Empty;
                                        for (int q = 0; q < len; q++) m_cardNo += byteHEX(szBytes[q]);
                                        Identifiant = m_cardNo;
                                        testlecture = 2;
                                    }
                                    else testlecture++;
                                }
                            }
                        }
                    }
                } while (testlecture < 2 && status == 0);

                Marshal.FreeHGlobal(pSnr);
            }
            return status;
        }

        public void PublishCardId(string cardId)
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Publish("rfid/reader", Encoding.UTF8.GetBytes(cardId), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            }
        }

        public string GetCardID()
        {
            int status = lireIdentifiantCarte();
            string statusMsg = null;
            switch (status)
            {
                case 0:
                    statusMsg = Identifiant;

                    // Publier l'ID de la carte sur MQTT
                    PublishCardId(Identifiant);
                    break;
                case 1:
                    MessageBox.Show("Erreur de vitesse du Port Série", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 2:
                    MessageBox.Show("Erreur de numéro du Port Série", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 20:
                    MessageBox.Show("Carte absente", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            return statusMsg;
        }

        public static string byteHEX(byte ib)
        {
            string _str = String.Empty;
            char[] Digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] ob = new char[2];
            ob[0] = Digit[(ib >> 4) & 0x0F];
            ob[1] = Digit[ib & 0x0F];
            _str = new String(ob);
            return _str;
        }
    }
}