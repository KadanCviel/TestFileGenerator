           
Test File Generator Ver 1.0 (2016-06-26)

Create Test File with binary data (e.g. pattern 0x00, 0x01, 0x02, ..., 0xff) with few bytes of index (1,2 or 3 bytes)


source file: MainWindow.xaml

if file size is
less then 128KB two bytes pattern   0000,0001,0002,.. ,00FF,
                                    0100.0101.0102,.. ,01FF,
                                    0200,0201,0202,.. ,02FF,
                                    :                  :
                                    FF00,FF01,FF02,.. ,FFFF 
 less then 48MB (more thren 128K) three bytes pattern 
                                    000000,000001,000002,.. ,0000FF
                                    000100,000101,000102,.. ,0001FF
                                    :
                                    00FF00,00FF01,00FF02,.. ,00FFFF
                                    010000,010001,010002,.. ,0100FF
                                    010100,010101,010102,.. ,0101FF
                                    :
                                    01FF00,01FF01,01FF02,.. ,01FFFF
                                    :
                                    :
                                    FF0000,FF0001,FF0002,.. ,FF00FF
                                    :
                                    :
                                    FFFF00,FFFF01,FFFF02,.. ,FFFFFF 
    more then 48MB four bytes pattern
                                    00000000,00000001,00000002,.. ,000000FF
                                    00000100,00000101,00000102,.. ,000001FF 
                                    :
                                    FF000000,FF000001,FF000002,.. ,FF0000FF 
                                    FFFF0000,FFFF0001,FFFF0002,.. ,FFFF00FF 

The file size border was calculated by
Pattern      Lines       * (bytes in line) =  
two byte     256         * (256*2)         = 2^8*2^8*2=2^17=2^7*1KBytes=128[KBytes]
three bytes* 256*256     * (256*3)         = 2^24*3=3*2^4*2^20= 3*16*1MBytes=48[MBytes]
four bytes   256*256*256 * (256*4)         = 2^32*4=2^30*4*4=16[GBytes] */