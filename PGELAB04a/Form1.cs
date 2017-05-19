using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGELAB04a
{
    public partial class Form1 : Form
    {
        newGame _newGame = new newGame();

        private Timer timer1;

        bool isTimer = false;
        bool gameOver = false;

        public static string userName = "Bunia";
        public static string cpuName = "CPU";


        Queue<int> userDec = new Queue<int>();
        Queue<int> cpuDec = new Queue<int>();
        Queue<int> drawDec = new Queue<int>();
        List<Image> numbersImage =new List<Image>();
        public static List<historyElement> history = new List<historyElement>();


        Image backImage = Image.FromFile("back.png");


        int cpuPoints = 0, userPoints = 0;
        public static int sets = 0;
        int numRound = 0;

        public static int maxRound = 10;
        int autoRound = 10;

        private Random _random = new Random();
        private bool isDraw;

        public Form1()
        {
           
            
          //  _newGame.ParentWindow = this;
            _newGame.ShowDialog(this);
            userName = _newGame._user;
            cpuName = _newGame._cpu;
            maxRound = _newGame._rounds;
            sets = _newGame._sets;
            autoRound = maxRound;

            InitializeComponent();
            

            label4.Text = cpuName;
            label5.Text = userName;
            label2.Text = "round " + numRound + " out of " + maxRound;
            autoBox.Text = autoRound.ToString();

            button1.Text = "";
            button4.Text = "";

            InitTimer();
            startNewGame();
            this.Hide();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                nextRound();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseGame();
        }

        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseGame();
        }


      
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameWIndow();

        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("War", "lab3b",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void NewGameWIndow()
        {
            _newGame.ShowDialog();
            
            userName = _newGame._user;
            cpuName = _newGame._cpu;
            maxRound = _newGame._rounds;
            sets = _newGame._sets;
            label4.Text = cpuName;
            label5.Text = userName;
            label2.Text = "round " + numRound + " out of " + maxRound;
            startNewGame();
        }
        #region save/load
        //https://msdn.microsoft.com/en-us/library/sfezx97z(v=vs.110).aspx

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Save game|*.Bunia";
            saveFileDialog1.Title = "Save a game";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false);
                file.WriteLine(userName );
                file.WriteLine(cpuName);
                file.WriteLine(numRound+"/"+maxRound);
                file.WriteLine(sets);
                

                Queue<int> userDecTemp = new Queue<int>(userDec);
                Queue<int> cpuDecTemp = new Queue<int>(cpuDec);
                while (userDecTemp.Count > 0)
                {

                    file.Write(userDecTemp.Dequeue()+";");
                }
                file.Write("\n");
                while (cpuDecTemp.Count > 0)
                {
                    file.Write(cpuDecTemp.Dequeue() + ";");
                }

                file.Write("\n");
               foreach(historyElement h in history)
                {
                    file.Write(h.userPoints + ";");
                }
                file.Write("\n");
                foreach (historyElement h in history)
                {
                    file.Write(h.cpuPoints + ";");
                }

               
                file.Close();
            }
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string[] line;
            string[] line1;




            openFileDialog1.Filter = "load game|*.Bunia";
            openFileDialog1.Title = "load a game";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {

                userDec.Clear();
                cpuDec.Clear();


                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.StreamReader file = new StreamReader(openFileDialog1.FileName);
                userName = file.ReadLine();
                cpuName = file.ReadLine();
                line = file.ReadLine().Split('/');

                int temp = 0;
                int tempUserHist = 0;
                int tempcpuHist = 0;
                int.TryParse(line[0],out numRound);
                int.TryParse(line[1], out maxRound);
                int.TryParse(file.ReadLine(), out sets);


                line = file.ReadLine().Split(';');
                foreach(string a in line)
                {
                    int.TryParse(a, out temp);
                    userDec.Enqueue(temp);
                }

                
                line = file.ReadLine().Split(';');
                foreach (string a in line)
                {
                    int.TryParse(a, out temp);
                    cpuDec.Enqueue(temp);
                }
                history.Clear();

                line = file.ReadLine().Split(';');
                line1 = file.ReadLine().Split(';');
                foreach (var a in line.Zip(line1, Tuple.Create))
                {
                    int.TryParse(a.Item1, out tempUserHist);
                    int.TryParse(a.Item2, out tempcpuHist);
                    history.Add(new historyElement(tempUserHist, tempcpuHist, 0));
                }

                label1.Text = userPoints.ToString();
                label3.Text = cpuPoints.ToString();
                label2.Text = "round " + numRound + " out of " + maxRound;
                gameOver = false;
                label4.Text = cpuName;
                label5.Text = userName;
                button2.BackColor = Color.Empty;
                button3.BackColor = Color.Empty;
                button2.BackgroundImage = base.BackgroundImage;
                button3.BackgroundImage = base.BackgroundImage;

                file.Close();
            }
        }
        #endregion

        
        
        


        #region karty
        private void startNewGame()
        {
            label4.Text = cpuName;
            label5.Text = userName;
            getNumerki();
            shufleCards();
            history.Clear();
            userPoints = userDec.Count();
            cpuPoints = cpuDec.Count();
            label1.Text = userPoints.ToString();
            label3.Text = cpuPoints.ToString();
            numRound = 0;
            gameOver = false;


            button2.BackColor = Color.Empty;
            button3.BackColor = Color.Empty;
            button2.BackgroundImage = base.BackgroundImage;
            button3.BackgroundImage = base.BackgroundImage;

            button2.Text = "";
            button3.Text = "";
            label1.Text = "0";
            label3.Text = "0";
            
            AutoButton.Text = "Start";
            if (isTimer)
                timer1.Stop();
            isTimer = false;



        }
        private void shufleCards()
        {
            userDec.Clear();
            cpuDec.Clear();
            List<int> cards = new List<int>();
            for (int i = 0; i < sets * 10; i++)
            {
                cards.Add(i % 10 + 1);
            }
            cards = cards.OrderBy(x => _random.Next()).ToList();
            for (int i = 0; i < sets * 10; i += 2)
            {
                userDec.Enqueue(cards[i]);
                cpuDec.Enqueue(cards[i + 1]);
            }

        }

        void getNumerki()
        {
            Image gifNumbers = Image.FromFile("numerki.gif");
            for (int i = 0; i < 10; i++)
            {
                numbersImage.Add(new Bitmap(100, gifNumbers.Height));
                var graphics = Graphics.FromImage(numbersImage[i]);
                graphics.DrawImage(gifNumbers, new Rectangle(0, 0, 100, gifNumbers.Height), new Rectangle(i * 100, 0, 100, gifNumbers.Height), GraphicsUnit.Pixel);
                graphics.Dispose();
            }
        }
        #endregion

        #region timer 
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            
        }

        public void Timer()
        {
            if (!isTimer)
            {
                AutoButton.Text = "stop";
                timer1.Interval = trackBar.Value;
                timer1.Start();
                isTimer = true;

            }
            else
            {
                timer1.Stop();
                isTimer = false;
                AutoButton.Text = "Start";
            }

        }

        private void AutoButton_Click(object sender, EventArgs e)
        {
            int.TryParse(autoBox.Text, out autoRound);
            Timer();
        }
        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Interval = trackBar.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(numRound<autoRound)
            if (!gameOver)
                nextRound();
        }
        #endregion

        #region gamePlay
        private void nextRound()
        {
            int cpuCard = 0, userCard = 0;
            if (userDec.Count() == 0 || cpuDec.Count() == 0)
                GameOver();
            cpuCard = userDec.Dequeue();
            userCard = cpuDec.Dequeue();


            //button2.Text = cpuCard.ToString();
            //button3.Text = userCard.ToString();
            button2.BackgroundImage = numbersImage[cpuCard - 1];
            button3.BackgroundImage = numbersImage[userCard - 1];

            numRound++;
            label2.Text = "round " + numRound + " out of " + maxRound;
            if (!isDraw)
            {
                if (userCard > cpuCard)
                {

                    
                    button2.BackColor = Color.Red;
                    button3.BackColor = Color.Green;


                    userDec.Enqueue(userCard);
                    userDec.Enqueue(cpuCard);
                    while (drawDec.Count > 0)
                    {

                        userDec.Enqueue(drawDec.Dequeue());
                    }

                    userPoints = userDec.Count();
                    cpuPoints = cpuDec.Count();
                    label1.Text = userPoints.ToString();
                    label3.Text = cpuPoints.ToString();
                }
                else
                {
                    if (userCard == cpuCard)
                    {
                        button2.BackColor = Color.Yellow;
                        button3.BackColor = Color.Yellow;

                        drawDec.Enqueue(userCard);
                        drawDec.Enqueue(cpuCard);
                        draw();


                    }
                    else
                    {

                        button2.BackColor = Color.Green;
                        button3.BackColor = Color.Red;


                        cpuDec.Enqueue(userCard);
                        cpuDec.Enqueue(cpuCard);
                        while (drawDec.Count>0)
                        {
                            
                                cpuDec.Enqueue(drawDec.Dequeue());
                        }

                        userPoints = userDec.Count();
                        cpuPoints = cpuDec.Count();
                        label1.Text = userPoints.ToString();
                        label3.Text = cpuPoints.ToString();
                    }

                }
                button2.Text = cpuCard.ToString();
                button3.Text = userCard.ToString();
            }
            else
            {
                button2.BackgroundImage = backImage;
                button3.BackgroundImage = backImage;

                button2.Text = "";
                button3.Text = "";

                drawDec.Enqueue(userCard);
                drawDec.Enqueue(cpuCard);
                isDraw = false;

            }


            history.Add(new historyElement(userPoints, cpuPoints, numRound));



            if (numRound == maxRound)
            {
                GameOver();

            }

        }

        void draw()
        {
            
            numRound++;
            isDraw = true;


        }

        void GameOver()
        {
            gameOver = true;

            System.IO.StreamWriter file = new System.IO.StreamWriter("scores.hights", true);

            file.WriteLine(userName + ";" + userPoints + ";" + numRound + ";" );
            file.Close();

            PostGame();
            MessageBox.Show(userPoints > cpuPoints ? "User win" : "cpu win");

            if (MessageBox.Show("Play agin?", "Play", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                NewGameWIndow();
            }

        }

        void PostGame()
        {
            if (MessageBox.Show("Post game stats?", "Stats", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                gameStats stats = new gameStats();
                stats.ShowDialog();
            }
        }

        private void topSocresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hightScores stats = new hightScores();
           
            stats.ShowDialog();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

      

        public static void CloseGame()
        {
            if (MessageBox.Show("Really close?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();

            }
        }
        #endregion
    }
}


