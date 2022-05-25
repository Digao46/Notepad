using System;
using System.IO;
using System.Windows.Forms;

namespace Notebad
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void novoDocumentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSecondary frmSecondary = new FrmSecondary();
            frmSecondary.Text = String.Format("Novo Documento - {0}", this.MdiChildren.Length + 1);
            frmSecondary.MdiParent = this;
            frmSecondary.Show();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Deseja mesmo sair?",
                 "Sair",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FrmSecondary activeDaughter = (FrmSecondary)this.ActiveMdiChild;

            if (activeDaughter != null)
            {
                try
                {
                    RichTextBox rtText = activeDaughter.rtUserText;

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Arquivo de Texto | *.txt";
                    saveFileDialog.FileName = "Arquivo.txt";
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                        for (int i = 0; i < rtText.Lines.Length; i++)
                        {
                            writer.WriteLine(rtText.Lines[i]);
                        }
                        activeDaughter.Text = saveFileDialog.FileName;

                        writer.Dispose();
                        writer.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Ops... Algo deu errado!");
                }
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Arquivo de Texto | *.txt"
            };


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FrmSecondary frmDaughter = new FrmSecondary();
                frmDaughter.MdiParent = this;
                StreamReader reader = new StreamReader(openFileDialog.OpenFile());

                frmDaughter.rtUserText.Text = reader.ReadToEnd();

                reader.Dispose();
                reader.Close();

                frmDaughter.Text = openFileDialog.FileName;
                frmDaughter.Show();
            }
        }

        private void copy_cut(bool cutting)
        {
            FrmSecondary frmDaughter = (FrmSecondary)this.ActiveMdiChild;

            if (frmDaughter != null)
            {
                try
                {
                    RichTextBox txtBox = frmDaughter.rtUserText;
                    if (txtBox != null)
                    {
                        Clipboard.SetText(txtBox.SelectedText, TextDataFormat.UnicodeText);
                        if (cutting)
                        {
                            txtBox.SelectedText = String.Empty;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erro ao copiar!");
                }
            }
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy_cut(false);
        }

        private void recortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy_cut(true);
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSecondary frmDaughter = (FrmSecondary)this.ActiveMdiChild;

            if (frmDaughter != null)
            {
                try
                {
                    RichTextBox txtBox = frmDaughter.rtUserText;
                    if (txtBox != null)
                    {
                        IDataObject data = Clipboard.GetDataObject();
                        if (data.GetDataPresent(DataFormats.StringFormat))
                        {
                            txtBox.SelectedText = data.GetData(DataFormats.StringFormat).ToString();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erro ao copiar!");
                }
            }
        }
    }
}

