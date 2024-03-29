using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using TestProject;
using ScottPlot;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace idealGasLaw
{
    public partial class Form1 : Form
    {
        const float R = 0.082f;
        /*
         * P = (vScrollBar2.Value * -0.001f + 1)*2                 //atm
         * V =     //L
         * T = vScrollBar1.Value * -1 + 546                         //K
         * n = trackBar1.Value*0.01f                               //mol
         * R = 0.082                                                //(atm*L) / (mol*K)
         */
        int T=273;
        float P=1, n=1;
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)//섭씨 체크
        {
            label5.Text = "273'C";
            label6.Text = "0'C";
            label7.Text = "-273'C";
            temperatureTextChange();
            CharlesLaw();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)//켈빈 체크
        {
            label5.Text = "546K";
            label6.Text = "273K";
            label7.Text = "0K";
            temperatureTextChange();
        }
        private void label19_Click(object sender, EventArgs e)
        {

        }
        private void label26_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        //===========================================================================================================================================================================
        private void trackBar1_Scroll(object sender, EventArgs e)//기체의 양 스크롤
        {
            label9.Text = n.ToString()+"mol";
            ValueUpdate();
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)//온도 스크롤
        {
            barColorChange();
            temperatureTextChange();
            ValueUpdate();
        }
        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)//압력 스크롤
        {
            barColorChange();
            label13.Text = P.ToString() + "atm";
            ValueUpdate();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)//실린더 부피 수정
        {
            barVolumeChange();
        }
        private void button1_Click_1(object sender, EventArgs e)//온도초기화
        {
            vScrollBar1.Value = 273;
            ValueUpdate();
            temperatureTextChange();
            barColorChange();
        }

        private void button2_Click(object sender, EventArgs e)//압력초기화
        {
            vScrollBar2.Value = 900;
            ValueUpdate();
            label13.Text = P.ToString() + "atm";
        }

        private void button3_Click(object sender, EventArgs e)//몰 초기화
        {
            trackBar1.Value = 100;
            ValueUpdate();
            label9.Text = n.ToString() + "mol";
        }
        void temperatureTextChange()
        {
            label8.Text = radioButton1.Checked ?(T-273).ToString() + "'C": T.ToString() + "K";
        }
        //===========================================================================================================================================================================
        void barColorChange()
        {
            stateProgressBar1.ForeColor = Color.FromArgb((int)(vScrollBar1.Value <= 273 ? (vScrollBar1.Value / -273f + 1) * 255 : 0), 0, (int)(vScrollBar1.Value >= 273 ? ((vScrollBar1.Value - 273) / 273f) * 255 : 0));
            stateProgressBar1.BackColor = Color.FromArgb((int)(vScrollBar2.Value *0.255f), (int)(vScrollBar2.Value * 0.255f), (int)(vScrollBar2.Value * 0.255f));
        }
        void barVolumeChange()
        {
            int V = (int)(n * R * T / P * 1000 / (int)numericUpDown1.Value);
            stateProgressBar1.Value = V < 1000 && V >= 0 ? V : 1000;
            label28.Text = (n * R * T / P).ToString() + "L";
        }
        void ValueUpdate()
        {
            P = vScrollBar2.Value * -0.01f + 10;
            T = vScrollBar1.Value * -1 + 546;
            n = trackBar1.Value * 0.01f;
            barVolumeChange();
            ShowGraph();
        }
        void ShowGraph()
        {
            BoyleLaw();
            CharlesLaw();
            AvogadroLaw();
        }
        //y축 = 부피, x축 = 나머지 변수
        void BoyleLaw()//PV=k   k=nRT
        {
            double k = n * R * T;
            label23.Text = Math.Round(k, 6).ToString() + "*atm*L";

            Plot plot = new Plot(800, 600);
            plot.AddVerticalLine(0, width: 2, color: Color.Black);
            plot.AddHorizontalLine(0, width: 2, color: Color.Black);

            plot.AddFunction(x => k / x, color: Color.Black);
            plot.AddVerticalLine(P, width: 2, color: Color.Red);
            plot.AddHorizontalLine(k/P, width: 2, color: Color.Blue);
            plot.SetAxisLimitsX(0, 10);
            plot.SetAxisLimitsY(0, 200);
            //plot.AddPoint(P, k / P);

            plot.Title("보일 법칙");
            plot.XLabel("압력:P(atm)");
            plot.YLabel("부피:V(L)");
            formsPlot1.Reset(plot);
            formsPlot1.Refresh();
            
        }
        void CharlesLaw()//V=kT k=nR/P
        {
            double k = n * R / P;
            label24.Text = Math.Round(k, 6).ToString() + "*L/K";

            Plot plot = new Plot(800, 600);
            plot.AddVerticalLine(0, width: 2, color: Color.Black);
            plot.AddHorizontalLine(0, width: 2, color: Color.Black);

            plot.AddFunction(x => k * (x + (radioButton1.Checked ? 273 : 0)), color: Color.Black);
            plot.AddVerticalLine(T - (radioButton1.Checked ? 273 : 0), width: 2, color: Color.Green);
            plot.AddHorizontalLine(T * k, width: 2, color: Color.Blue);
            plot.SetAxisLimitsX(radioButton1.Checked ? -273 : 0, radioButton1.Checked ? 327 : 600);
            plot.SetAxisLimitsY(0, 200);

            plot.Title("샤를 법칙");
            plot.XLabel(radioButton1.Checked ? "온도:T('C)" : "온도:T(K)");
            plot.YLabel("부피:V(L)");
            formsPlot2.Reset(plot);
            formsPlot2.Refresh();
        }

        

        void AvogadroLaw()//V=kn k=RT/P
        {;
            
            double k = R * T / P;
            label25.Text = Math.Round(k,6).ToString() + "*L/mol";

            Plot plot = new Plot(800, 600);
            plot.AddVerticalLine(0, width: 2, color: Color.Black);
            plot.AddHorizontalLine(0, width: 2, color: Color.Black);

            plot.AddFunction(x => k * x, color: Color.Black);
            plot.AddVerticalLine(n, width: 2, color: Color.Purple);
            plot.AddHorizontalLine(n * k, width: 2, color: Color.Blue);
            plot.SetAxisLimitsX(0, 10);
            plot.SetAxisLimitsY(0, 200);

            plot.Title("아보가드로 법칙");
            plot.XLabel("양:n(mol)");
            plot.YLabel("부피:V(L)");
            formsPlot3.Reset(plot);
            formsPlot3.Refresh();
        }
    }
}
