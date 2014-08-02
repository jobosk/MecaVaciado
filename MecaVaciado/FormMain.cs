using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace MecaVaciado
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void calcular(Double x, Double y, Double a, Double b, Double xp, Double yp, Double ap, Double bp, Double z, Double r, Double f, Boolean isla)
        {
            Double zo = 0;

            // Algun dato es incorrecto
            if (a <= 0 || b <= 0 || z >= zo || r <= 0 || f <= 0)
                return;

            // La herramienta no cabe en el hueco
            if ((2 * r) > a || (2 * r) > b)
                return;

            Int32 nv = (int) (Math.Abs(z) / f);
            Double rv = Math.Abs(z) % f;

            Int32 nh = (int) (a / (2 * r));
            Double rh = a % (2 * r);

            if (isla)
            {
                // Algun dato es incorrecto
                if (ap <= 0 || bp <= 0)
                    return;

                // La isla está fuera del hueco en el eje de las X
                if (xp <= x || xp >= (x + a))
                    return;

                // La isla esta fuera del hueco en el eje de las Y
                if (yp <= y || yp >= (y + b))
                    return;

                // La isla es más grande que el hueco en alguna dimension
                if (ap > a || bp > b)
                    return;

                // La herramienta no cabe en el hueco entre la pared y la isla
                if ((2 * r) > (xp - x) || (2 * r) > (yp - y))
                    return;
            }

            SaveFileDialog salvar = new SaveFileDialog();

            //salvar.Filter = "Archivos ISO (*.iso)|*.iso";
            //salvar.Filter = "Archivos DXF (*.dxf)|*.dxf";
            salvar.Filter = "Archivos de texto (*.txt)|*.txt";
            salvar.FilterIndex = 2;

            if (salvar.ShowDialog() != DialogResult.OK)
                return;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(salvar.FileName, false))
            {
                //file.WriteLine("N10 G54");

                //file.WriteLine("N20 G17 G90 G71 G94");

                //file.WriteLine("N30 T1 D1 M06 S2000 F400");

                //file.WriteLine("N40 M3");

                Int32 n = 10;

                Double i = x + r;
                Double j = y + r;
                Double k = 0;

                file.WriteLine("N" + n + " G00 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                n += 5;

                // Contorno del hueco
                
                for (int p = 0; p <= nv; p++)
                {
                    if (p == nv)
                    {
                        if (rv == 0)
                            break;

                        k -= rv;
                    }
                    else
                        k -= f;

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    i += a - (2 * r);

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    j += b - (2 * r);

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    i -= a - (2 * r);

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    j -= b - (2 * r);

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                    n += 5;
                }

                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                n += 5;

                if (isla)
                {
                    i = xp - r;
                    j = yp - r;

                    file.WriteLine("N" + n + " G00 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    // Contorno de la isla

                    k = 0;

                    for (int p = 0; p <= nv; p++)
                    {

                        if (p == nv)
                        {
                            if (rv == 0)
                                break;

                            k -= rv;
                        }
                        else
                            k -= f;

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;

                        i += ap + (2 * r);

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;

                        j += bp + (2 * r);

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;

                        i -= ap + (2 * r);

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;

                        j -= bp + (2 * r);

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;
                    }

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                    n += 5;
                }

                // Vaciado

                k = 0;

                for (int p = 0; p <= nv; p++)
                {
                    i = x + r;
                    j = y + r;

                    bool dir = true;

                    file.WriteLine("N" + n + " G00 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                    n += 5;

                    if (p == nv)
                    {
                        if (rv == 0)
                            break;

                        k -= rv;
                    }
                    else
                        k -= f;

                    for (int q = 0; q <= nh; q++)
                    {
                        if (q == nh)
                        {
                            if (rh == 0)
                                break;

                            i += rh;
                        }
                        else if (q > 0)
                            i += (2 * r);

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;

                        if (dir)
                        {
                            j += b - (2 * r);

                            if (isla && i > (xp - r) && i < (xp + ap + r))
                            {
                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp - r).ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp - r).ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp + bp + r).ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp + bp + r).ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                                n += 5;
                            }
                        }
                        else
                        {
                            j -= b - (2 * r);

                            if (isla && i > (xp - r) && i < (xp + ap + r))
                            {
                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp + bp + r).ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp + bp + r).ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp - r).ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                                n += 5;

                                file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + (yp - r).ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                                n += 5;
                            }
                        }

                        dir = !dir;

                        file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + k.ToString(CultureInfo.InvariantCulture));
                        n += 5;
                    }

                    file.WriteLine("N" + n + " G01 X" + i.ToString(CultureInfo.InvariantCulture) + " Y" + j.ToString(CultureInfo.InvariantCulture) + " Z" + zo.ToString(CultureInfo.InvariantCulture));
                    n += 5;
                }

                //file.WriteLine("N" + n + " M5");
                //n += 5;

                //file.WriteLine("N" + n + " M30");
                //n += 5;
            }
        }

        private void buttonCalcular_Click(object sender, EventArgs e)
        {
            Double x, y, a, b;
            Double xp, yp, ap, bp;
            Double z, r, f;

            Boolean isla;

            Double.TryParse(this.textBoxX.Text, out x);
            Double.TryParse(this.textBoxY.Text, out y);
            Double.TryParse(this.textBoxA.Text, out a);
            Double.TryParse(this.textBoxB.Text, out b);

            isla = this.checkBoxIsla.Checked;

            if (isla)
            {
                Double.TryParse(this.textBoxXP.Text, out xp);
                Double.TryParse(this.textBoxYP.Text, out yp);
                Double.TryParse(this.textBoxAP.Text, out ap);
                Double.TryParse(this.textBoxBP.Text, out bp);
            }
            else
            {
                xp = yp = ap = bp = 0.0;
            }

            Double.TryParse(this.textBoxZ.Text, out z);
            Double.TryParse(this.textBoxR.Text, out r);
            Double.TryParse(this.textBoxF.Text, out f);

            calcular(x, y, a, b, xp, yp, ap, bp, z, r, f, isla);
        }

        private void buttonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
