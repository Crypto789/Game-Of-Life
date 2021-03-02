using System;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace Project_Portfolio
{
    public partial class Form1 : Form
    {

        bool[,] universe = new bool[50, 50];
        
        bool[,] scratchPad = new bool[50, 50];       

        Color gridColor = Color.DarkGreen;
        Color cellColor = Color.DarkGreen;

        Timer timer = new Timer();

        int generations = 0;
        int Cells = 0;
        public Form1()
        {
            InitializeComponent();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
   

            timer.Interval = 100; 
            timer.Tick += Timer_Tick;
            timer.Enabled = false; 

        }
        private void NextGeneration()
        {
            
            int count;
            int count2;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    count = CountNeighborsFinite(x, y);
                    count2 = CountNeighborsToroidal(x, y);
                    bool live = false;
                    bool isAlive = universe[x, y];
                    if (isAlive && (count > 2))
                    {
                        live = false;
                       
                       
                        graphicsPanel1.Invalidate();
                        
                    }
                    if (isAlive && (count > 3))
                    {
                        live = false;
                       
                        graphicsPanel1.Invalidate();
                       
                    }
                    if (isAlive && (count == 2 || count == 3))

                    {
                        
                        live = true;
                        
                        graphicsPanel1.Invalidate();
                        
                    }
                    else if (!isAlive && count == 3)

                    {
                       
                        live = true;
                        
                        graphicsPanel1.Invalidate();
                      
                    }
                    if (isAlive && (count2 > 2))
                    {
                       
                        live = false;
                       
                        graphicsPanel1.Invalidate();
                       
                    }
                    if (isAlive && (count2 > 3))
                    {
                        
                        live = false;
                        
                        graphicsPanel1.Invalidate();
                       
                    }
                    if (isAlive && (count2 == 2 || count2 == 3))

                    {
                        live = true;
                       
                        graphicsPanel1.Invalidate();
                       
                    }
                    else if (!isAlive && count2 == 3)

                    {   
                        live = true;
                       
                        graphicsPanel1.Invalidate();
                       
                    }
                    scratchPad[x, y] = live;
                }
            }
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;
            generations++;
            
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            

        }
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;

                    }
                    if (xCheck < 0)
                    {
                        continue;

                    }

                    if (yCheck < 0)
                    {
                        continue;
                    }

                    if (xCheck >= xLen)
                    {
                        continue;
                    }

                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }

                }
            }
            return count;
        }
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    if (xOffset == 0 && yOffset == 0) { continue; }

                    if (xCheck < 0)
                    {
                        return xLen - 1;

                    }
                    if (yCheck < 0)
                    {
                        return yLen - 1;

                    }
                    if (xCheck >= xLen)
                    {
                        return 0;

                    }
                    if (yCheck >= yLen)
                    {
                        return 0;

                    }
                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            return count;

        }
        private void Timer_Tick(object sender,  EventArgs e)
        {
            NextGeneration();
        }
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            float cellWidth = ((float)graphicsPanel1.ClientSize.Width / universe.GetLength(0));

            float cellHeight = ((float)graphicsPanel1.ClientSize.Height / universe.GetLength(1));

            Pen gridPen = new Pen(gridColor, 1);
           
             Brush cellBrush = new SolidBrush(cellColor);
              
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    RectangleF cellRect = RectangleF.Empty;
                     cellRect.X = x * cellWidth;
                     cellRect.Y = y * cellHeight;
                     cellRect.Width = cellWidth;
                     cellRect.Height = cellHeight;
                    
                    if (universe[x, y] == true)
                    {
                        
                        e.Graphics.FillRectangle(cellBrush, cellRect);

                        

                    }
                    
                    e.Graphics.DrawRectangle( gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                   
                }
                
            }
            Cells++;
            toolStripStatusLabel1.Text = "Alive= " +Cells .ToString();
           
            gridPen.Dispose();
            cellBrush.Dispose();
        }
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float cellWidth = ((float)graphicsPanel1.ClientSize.Width / universe.GetLength(0));
                float cellHeight = ((float)graphicsPanel1.ClientSize.Height / universe.GetLength(1));
                float x = e.X / cellWidth;
                int X = ((int)x);
                float y = e.Y / cellHeight;
                int Y = ((int)y);

                universe[X, Y] = !universe[X, Y];
              
                graphicsPanel1.Invalidate();
            }
        }
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog()) { graphicsPanel1.BackColor = dlg.Color; }

        }
        private void modalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();
            int number=0;
            universe = new bool [number,number];
            
            dlg.timer = timer.Interval;
            dlg.Universe = number;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                number = (int)dlg.Universe;
                timer.Interval = (int)dlg.timer;
                universe = new bool[number, number];
                graphicsPanel1.Invalidate();
            }
             
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.Save();
        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
        }
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
        }
        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog()) { graphicsPanel1.BackColor = dlg.Color; }
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

            timer.Enabled = true;
        }
       
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {


                    universe[x, y] = false;


                }
            }
            graphicsPanel1.Invalidate();
        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {

            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {


                    universe[x, y] = false;


                }
            }
            graphicsPanel1.Invalidate();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            timer.Enabled = true;

        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        private void graphicsPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculating the width and height of each cell in pixels
                float cellWidth = ((float)graphicsPanel1.ClientSize.Width / universe.GetLength(0));
            
                float x = e.X / cellWidth;
                int X = ((int)x);
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;
                int Y = ((int)y);
               
                universe[X, Y] = !universe[X, Y];

                //repainting windows
                graphicsPanel1.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";
            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                
                writer.WriteLine("!DATA.");

                // Iterating through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                 {

                    // Creating a string to represent the current row.
                    StringBuilder currentRow = new StringBuilder();

                    // Iterating through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                       
                        if (universe[x, y] == true)
                        {
                            currentRow.Append('O');
                        }

                        
                        else if (universe[x, y] == false)
                        {
                            currentRow.Append(',');
                        }
                    }

                    
                    writer.WriteLine(currentRow);
                 }

               
                writer.Close();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                //variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterating through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Reading one row at a time.
                    string row = reader.ReadLine();
                    
                            if (row[0] == '!')
                            {
                                 
                               
                                 if (row.Length != '!')
                                 {
                                     maxHeight++;
                                    
                                    maxWidth = row.Length;

                                 }
                            }
            
                }

                
                bool[,] Temp = new bool[maxWidth, maxHeight];
             
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    int y = 0;
                           
                            if (row[0] == '!')
                            {
                                    if (row.Length != '!')
                                    {
                                         for (int xPos = 0; xPos < row.Length; xPos++)
                                         {
                                               if (row[xPos] == 'O')
                                               {
                                                   Temp[xPos,y ] = true;
                                               }
                                                
                                               if (row[xPos] == '.')
                                               {
                                                   Temp[xPos, y] = false;
                                               }

                                         }
                                    }
                            }
                           
                        
                }

 
                reader.Close();
            }
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Random random = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
               
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (random.Next(0, 50) == 0)
                    {
                        universe[x, y]= true;
                        generations++;
                        graphicsPanel1.Invalidate();
                    }
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            
        }
    } 
}
