using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Int32 length=123;
        Int64 length_in_bytes=456;

        string file_name = "temp.bin";

        public MainWindow()
        {
            InitializeComponent();
            BytesButton.IsChecked = true;
        }


        /// <summary>
        /// FUNCTION:     read_size
        /// DESCRIPTION:  read value from TextBox and convert it to int64
        /// PARAMS:       none
        /// RETURN VALUE: bool (TRUE - can parse from text to int, FALSE cant parse fron Text to int)
        /// NOTES:
        /// </summary>
        bool read_size()
        {

            string theText = textBox1.Text;

            bool parsed = Int32.TryParse(theText, out length);

            if (!parsed)
            {
                // Console.WriteLine("Int32.TryParse could not parse '{0}' to an int.\n", theText);
                // Output: Int32.TryParse could not parse 'abc' to an int.
                MessageBox.Show("Int32.TryParse could not parse "+ theText + " to an int.");
            }
            else
            {
                //MessageBox.Show(theText);
                //MessageBox.Show(length.ToString());
            }

            return parsed;

        }/// end read_size()


        /// <summary>
        /// FUNCTION:     button_Click
        /// DESCRIPTION:  button manager
        /// PARAMS:       object sender, RoutedEventArgs e
        /// RETURN VALUE: none
        /// NOTES:
        /// </summary>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool parse_text_box = false;

            if (BytesButton.IsChecked == true)
            {
                parse_text_box = read_size();
                if (parse_text_box == true)
                {
                    length_in_bytes = length;
                }
            }
            else if (KiloBytesButton.IsChecked == true)
            {
                parse_text_box = read_size();
                if (parse_text_box == true)
                {
                    length_in_bytes = length*1024;
                }
            }
            else if (MegaBytesButton.IsChecked == true)
            {
                parse_text_box = read_size();
                if (parse_text_box == true)
                {
                    length_in_bytes = length * 1024 * 1024;
                }
            }
            else 
            {
                MessageBox.Show("Errors - no button selected");
            }

            /* now when we know the length we can start fill the file 
             * if file size is
             * less then 128KB two bytes pattern 0000,0001,0002,.. ,00FF,
             *                                   0100.0101.0102,.. ,01FF,
             *                                   0200,0201,0202,.. ,02FF,
             *                                   :                  :
             *                                   FF00,FF01,FF02,.. ,FFFF 
             * less then 48MB (more thren 128K) three bytes pattern 
             *                                    000000,000001,000002,.. ,0000FF
             *                                    000100,000101,000102,.. ,0001FF
             *                                    :
             *                                    00FF00,00FF01,00FF02,.. ,00FFFF
             *                                    010000,010001,010002,.. ,0100FF
             *                                    010100,010101,010102,.. ,0101FF
             *                                    :
             *                                    01FF00,01FF01,01FF02,.. ,01FFFF
             *                                    :
             *                                    :
             *                                    FF0000,FF0001,FF0002,.. ,FF00FF
             *                                    :
             *                                    :
             *                                    FFFF00,FFFF01,FFFF02,.. ,FFFFFF 
             *   more then 48MB four bytes pattern
             *                                   00000000,00000001,00000002,.. ,000000FF
             *                                   00000100,00000101,00000102,.. ,000001FF 
             *                                   :
             *                                   FF000000,FF000001,FF000002,.. ,FF0000FF 
             *                                   FFFF0000,FFFF0001,FFFF0002,.. ,FFFF00FF 
             * The file size border was calculated by
             * Pattern      Lines       * (bytes in line) =  
             * two byte     256         * (256*2)         = 2^8*2^8*2=2^17=2^7*1KBytes=128[KBytes]
             * three bytes* 256*256     * (256*3)         = 2^24*3=3*2^4*2^20= 3*16*1MBytes=48[MBytes]
             * four bytes   256*256*256 * (256*4)         = 2^32*4=2^30*4*4=16[GBytes] */


            if (parse_text_box == true)//only if size parse from test box successfuly.
            {
                file_name = @textBox.Text;

                ///  http://stackoverflow.com/questions/4999988/to-clear-the-contents-of-a-file
                if (File.Exists(file_name))
                {
                    System.IO.File.WriteAllText(file_name, string.Empty);
                }

                try
                {

                    /// http://stackoverflow.com/questions/24357982/c-write-values-into-binary-bin-file-format
                    var sw = File.OpenWrite(file_name);
                    var bw = new BinaryWriter(sw);
                    byte vr = 0x00;

                    if (length_in_bytes < 128 * 1024) //128[KBytes] two bytes pattern
                    {
                        for (Int64 i = 0; i < length_in_bytes / 2; i++)
                        {
                            vr = (byte)(i >> 8);
                            bw.Write((byte)vr);
                            vr = (byte)i;
                            bw.Write((byte)vr);
                        }/* End for (Int64 i = 0; i < length_in_bytes/2; i++) */
                    }/* End if (length_in_bytes < 128 * 1024) */
                    else if (length_in_bytes < 48 * 1024 * 1024) //48 [MBytes] three bytes patten
                    {
                        for (Int64 i = 0; i < length_in_bytes / 3; i++)
                        {
                            vr = (byte)(i >> 16);
                            bw.Write((byte)vr);
                            vr = (byte)(i >> 8);
                            bw.Write((byte)vr);
                            vr = (byte)i;
                            bw.Write((byte)vr);
                        }/* End for (Int64 i = 0; i < length_in_bytes/3; i++) */
                    }/* End else if (length_in_bytes < 48 * 1024 * 1024) */
                    else //works perferct up to 16 [GBytes] four bytes pattern
                    {
                        for (Int64 i = 0; i < length_in_bytes / 4; i++)
                        {
                            vr = (byte)(i >> 24);
                            bw.Write((byte)vr);
                            vr = (byte)(i >> 16);
                            bw.Write((byte)vr);
                            vr = (byte)(i >> 8);
                            bw.Write((byte)vr);
                            vr = (byte)i;
                            bw.Write((byte)vr);
                        }/* End for (Int64 i = 0; i < length_in_bytes/4; i++) */
                    }/* End else */

                    bw.Flush();
                    bw.Close();
                    MessageBox.Show("File ( " + file_name + " ) with " + length_in_bytes.ToString() + " [Bytes] created.");
                }
                catch (System.UnauthorizedAccessException error_e)
                {
                    //System.Console.WriteLine(e.Message);
                    MessageBox.Show("File ( " + file_name + " ) UnauthorizedAccessExceptio " + error_e.Message);
                }
                finally
                {
                   /* TBD */
                }
            }/* End if (parse_text_box == true) */
        }/*End private void button_Click(object sender, RoutedEventArgs e) */


        /* Help button */
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Two bytes pattern for files less then 128[KBytes]\nThree bytes pattern for files less then 48[MByes]\nelse Four bytes pattern (repeat after 16[GBytes]).");
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }/* End public partial class MainWindow : Window */
}/*End namespace WpfApplication1 */
