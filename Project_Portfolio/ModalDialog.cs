using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Portfolio
{
    public partial class ModalDialog : Form
    {
        public ModalDialog()
        {
            InitializeComponent();
        }

        public decimal timer 
        {
            get 
            {
                return numericUpDownTimer.Value;
            }

            set 
            {
                numericUpDownTimer.Value = value;
            }
        }

        public decimal Universe
        {
            get
            {
                return Size.Value;
            }

            set
            {
                Size.Value = value;
            }
        }

       
    }
}
