#region Libraries
using System;
using System.Drawing;
#endregion

namespace Steganografi
{
    class StegoIslem
    {
        #region Enum
        //Enum serves to make sense and keep the constants that I will specify in the program.
        public enum State
        {
            encrypt,zeroLenght
        };
        #endregion

        #region Text Encrypt Code

        public static Bitmap hide_text(string text,Bitmap bmp)
        {
            State state = State.encrypt; // Initially, I hide the characters in the picture.
            int charIndex = 0; //holds the index of the character that is being hidden

            int charValue = 0; // holds the value of the character converted to integer

            long pixelElementIndex = 0;// holds the number of trailing zeros that have been added when finishing the process

            int zero = 0;// holds the number of trailing zeros that have been added when finishing the process.

            int R = 0, G = 0, B = 0;// hold pixel elements

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j,i);// holds the pixel that is currently being processed
                    R = pixel.R - pixel.R % 2;// now, clear the least significant bit(LSB) from each pixel element . 
                                              //One bit to the left. If the value of 010001 is 101, then the value of 010000 will be 100.
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    for (int n = 0; n < 3; n++) //I will examine and process each pixel element.
                    {
                        if(pixelElementIndex % 8 == 0)// check if new 8 bits has been processed
                        {
                            if (state == State.zeroLenght && zero == 8)//Add 8 zeros when 8 bits are processed. This way I will only have seen the message while it is resolved.
                            {
                                if ((pixelElementIndex -1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));//The values of the incoming characters are written.
                                }
                                return bmp;//return the bitmap with the text hidden in
                            }
                            if (charIndex>=text.Length)//check if all characters has been hidden
                            {
                                state = State.zeroLenght;// start adding zeros to mark the end of the text
                            }
                            else
                            {
                                charValue = text[charIndex++]; // move to the next character and process again

                                Console.WriteLine(charValue);
                            }
                        }
                        
                        switch(pixelElementIndex % 3)// check which pixel element has the turn to hide a bit in its LSB.
                        {
                            case 0:
                                {
                                    if (state == State.encrypt)
                                    {
                                        R += charValue % 2;/*  the rightmost bit in the character will be (charValue % 2)
                                         to put this value instead of the LSB of the pixel element
                                         just add it to it.*/

                                        charValue /= 2;

                                        Console.WriteLine(R.ToString());// removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                    }
                                    break;
                                }

                            case 1:
                                {
                                    if(state==State.encrypt)
                                    {
                                        G += charValue % 2;
                                        charValue /= 2;

                                        Console.WriteLine(G.ToString());
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    if (state == State.encrypt)
                                    {
                                        B += charValue % 2;
                                        charValue /= 2;

                                        Console.WriteLine(B.ToString());
                                    }
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;
                        if (state==State.zeroLenght)
                        {
                            // increment the value of zeros until it is 8
                            zero++;
                        }
                    }

                }
                
            }

        return bmp;
            #endregion
        }




        #region Decrypt the Message
        public static string decrypt (Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charVal = 0;
            string extractText = "";// holds the text that will be extracted from the image
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    for (int n = 0; n < 3; n++)// for each pixel, pass through its elements (RGB)
                    {
                        switch(colorUnitIndex % 3)
                        {
                            /*
                              get the LSB from the pixel element (will be pixel.R % 2)
                              then add one bit to the right of the current character
                              this can be done by (charValue = charValue * 2)
                              replace the added bit (which value is by default 0) with
                              the LSB of the pixel element, simply by addition
                             */
                            case 0:
                                {
                                    charVal = charVal * 2 + pixel.R % 2;//The least significant bit found in the red pixel. This bit is the processed bit.
                                }
                                break;
                            case 1:
                                {
                                    charVal = charVal * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charVal = charVal * 2 + pixel.B % 2;
                                }
                                break;
                        }
                        colorUnitIndex++;

                        //if 8 bits has been added, then add the current character to the result text

                        if (colorUnitIndex%8==0)
                        {
                            charVal = reverseBits(charVal);// reverse? of course, since each time the process happens on the right (for simplicity)
                            //
                            Console.WriteLine(charVal);//how many????
                            //

                            if (charVal==0)
                            {
                                return extractText;
                            }
                            #region To Avoid Turkish Character Error
                            if (charVal == 94)/*Ü=220 Ç=*/
                            {
                                charVal = 350;

                            }
                            else if (charVal == 95)
                            {
                                charVal = 351;
                            }
                            else if (charVal == 48)
                            {
                                charVal = 304;
                            }
                            #endregion
                            char c = (char)charVal;// convert the character value from int to char

                            extractText += c.ToString();// add the current character to the result text
                        }
                    }
                }
                
            }

            return extractText;
        }

    

    public static int reverseBits(int n)
    {
        int result = 0;
        for (int i = 0; i < 8; i++)
        {
            result = result * 2 + n % 2;
            n /= 2;
        }
        return result;
    }
  }
#endregion
}

