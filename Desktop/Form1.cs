using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Forms;
using QDocument = QuestPDF.Fluent.Document;
using QContainer = QuestPDF.Infrastructure.IContainer;


namespace Desktop
{
    public partial class Form1 : Form
    {
        string csvPath = "";
        string logoPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnPilihCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                csvPath = ofd.FileName;
                lblCSV.Text = csvPath;
            }
        }

        private void btnPilihLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                logoPath = ofd.FileName;
                lblLogo.Text = logoPath;
            }
        }

        private void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(csvPath) || string.IsNullOrEmpty(logoPath))
            {
                MessageBox.Show("Pilih file CSV dan logo terlebih dahulu!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files|*.pdf";
            sfd.FileName = "rekap_semua_dosen.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string outputPdf = sfd.FileName;
                GeneratePDF(csvPath, logoPath, outputPdf);
                MessageBox.Show("PDF berhasil dibuat:\n" + outputPdf);
            }
        }

        private void GeneratePDF(string csvPath, string logoPath, string outputPdf)
        {
            var records = new List<dynamic>();
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<dynamic>().ToList();
            }

            var headers = ((IDictionary<string, object>)records[0]).Keys.ToList();
            string kolDosen = "Nama Dosen";
            string kolMatkul = "Mata Kuliah";
            string kolSaran = headers.FirstOrDefault(h => h.Contains("Sampaikan saran/masukan"));
            var pertanyaanCols = headers
                .Where(h => h != "Response ID" && h != kolDosen && h != kolMatkul && h != kolSaran)
                .ToList();

            var nilaiMap = new Dictionary<string, double>
            {
                ["Sangat Setuju"] = 4,
                ["Setuju"] = 3,
                ["Cukup Setuju"] = 2,
                ["Tidak Setuju"] = 1
            };

            var groups = records.GroupBy(r =>
            {
                var dict = (IDictionary<string, object>)r;
                return (dict[kolDosen]?.ToString(), dict[kolMatkul]?.ToString());
            });

            QDocument.Create(container =>
            {
            container.Page(page =>
            {
            page.Margin(30);
            page.Size(PageSizes.A4);

            page.Content().Column(col =>
            {
            foreach (var group in groups)
            {
                var (dosen, matkul) = group.Key;
                    col.Item().PaddingBottom(10).Row(row =>
                    {
                        row.RelativeItem(1).Height(60).Image(logoPath, ImageScaling.FitHeight);
                        row.RelativeItem(4).Column(headerCol =>
                        {
                            headerCol.Item().Text(t =>
                                t.Span("Evaluasi Dosen oleh Taruna").FontSize(16).Bold().FontColor("#003366"));
                            headerCol.Item().Text(t =>
                                t.Span("Politeknik Siber dan Sandi Negara | Jurusan Kriptografi").FontSize(12).FontColor("#003366"));
                            headerCol.Item().Text(t =>
                                t.Span("Semester Gasal T.A. 2024/2025").FontSize(12).FontColor("#003366"));
                            headerCol.Item().PaddingTop(8).Text(t =>
                                t.Span($"{dosen}\n{matkul}").FontSize(12).Bold().FontColor("#003366"));
                        });
                    });


                    col.Item().PaddingBottom(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyleHeader).Text("Aspek/Pertanyaan");
                            header.Cell().Element(CellStyleHeader).Text("Nilai");
                            header.Cell().Element(CellStyleHeader).Text("Nilai Rata-\nRata Kelas");
                        });

                        foreach (var pertanyaan in pertanyaanCols)
                        {
                            var nilaiList = group
                                .Select(r => ((IDictionary<string, object>)r)[pertanyaan]?.ToString())
                                .Where(v => nilaiMap.ContainsKey(v))
                                .Select(v => nilaiMap[v])
                                .ToList();
                            double rata = nilaiList.Any() ? Math.Round(nilaiList.Average(), 2) : 0;

                            var nilaiKelasList = records
                                .Select(r => ((IDictionary<string, object>)r)[pertanyaan]?.ToString())
                                .Where(v => nilaiMap.ContainsKey(v))
                                .Select(v => nilaiMap[v])
                                .ToList();
                            double rataKelas = nilaiKelasList.Any() ? Math.Round(nilaiKelasList.Average(), 2) : 0;

                            string pertanyaanSingkat = pertanyaan;
                            var match = System.Text.RegularExpressions.Regex.Match(pertanyaan, @"\[(.*?)\]$");
                            if (match.Success)
                                pertanyaanSingkat = match.Groups[1].Value;

                            table.Cell().Element(CellStyle).Text(pertanyaanSingkat);
                            table.Cell().Element(CellStyle).Text(rata.ToString("0.##"));
                            table.Cell().Element(CellStyle).Text(rataKelas.ToString("0.##"));
                        }
                    });


                    if (!string.IsNullOrEmpty(kolSaran))
                    {
                        var saranList = group
                            .Select(r => ((IDictionary<string, object>)r)[kolSaran]?.ToString())
                            .Where(s => !string.IsNullOrWhiteSpace(s) && s != "-")
                            .ToList();
                        if (saranList.Any())
                        {
                            col.Item().Element(container =>
                            {
                                container.PaddingBottom(4);
                                container.Text("Saran/masukan dari Taruna")
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor("#003366");
                            });


                            col.Item().Table(saranTable =>
                            {
                                saranTable.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(30);
                                    cols.RelativeColumn();
                                });
                                saranTable.Header(header =>
                                {
                                    header.Cell().Element(CellStyleHeader).Text("No");
                                    header.Cell().Element(CellStyleHeader).Text("Saran/Masukan");
                                });
                                int no = 1;
                                foreach (var saran in saranList)
                                {
                                    saranTable.Cell().Element(CellStyle).Text(no.ToString());
                                    saranTable.Cell().Element(CellStyle).Text(saran);
                                    no++;
                                }
                            });
                        }
                    }

                    if (!group.Equals(groups.Last()))
                    {
                        col.Item().Element(x => x.PageBreak());


                    }
                }
            });
            });
            }).GeneratePdf(outputPdf);
        }

        static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) =>
            container.Border(1).BorderColor("#003366").Padding(4);



        static QuestPDF.Infrastructure.IContainer CellStyleHeader(QuestPDF.Infrastructure.IContainer container) =>
    container.Background("#003366").Padding(4).DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.White).Bold());


    }
}