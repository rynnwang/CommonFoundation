using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Beyova.InternalUtil
{
    internal static class Core
    {
        /// <summary>
        /// Does the encrypt.
        /// </summary>
        /// <param name="inputTextBox">The input text box.</param>
        /// <param name="outputTextBox">The output text box.</param>
        internal static void DoEncrypt(this TextBox inputTextBox, TextBox outputTextBox)
        {
            if (inputTextBox != null && outputTextBox != null)
            {
                outputTextBox.Text = inputTextBox.Text.EncryptR3DES();
            }
        }

        /// <summary>
        /// Does the decrypt.
        /// </summary>
        /// <param name="outputTextBox">The output text box.</param>
        /// <param name="inputTextBox">The input text box.</param>
        internal static void DoDecrypt(this TextBox outputTextBox, TextBox inputTextBox)
        {
            if (inputTextBox != null && outputTextBox != null)
            {
                try
                {
                    inputTextBox.Text = outputTextBox.Text.DecryptR3DES();
                }
                catch
                {
                    inputTextBox.Clear();
                }
            }
        }
    }
}
