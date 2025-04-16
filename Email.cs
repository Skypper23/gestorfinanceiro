using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit;
using MimeKit;
using System.Linq;
using System.Text.RegularExpressions;

namespace GestorFinanceiro
{
    public class Email
    {
        HistoricoManager hm = new HistoricoManager();

        public void VerificarEmail()
        {
            using (var client = new ImapClient())
            {
                // Liga-te à conta
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate("joaotestecsharp@gmail.com", "alhm euco vodv jklm");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite); // Para poder marcar como lido

                var uids = inbox.Search(SearchQuery.NotSeen); // Só mensagens novas

                foreach (var uid in uids)
                {
                    var mensagem = inbox.GetMessage(uid);
                    string corpo = mensagem.TextBody?.ToLower();

                    if (corpo != null && (corpo.Contains("dinheiro", StringComparison.OrdinalIgnoreCase) || corpo.Contains("ganhos", StringComparison.OrdinalIgnoreCase) || corpo.Contains("despesas", StringComparison.OrdinalIgnoreCase) || corpo.Contains("debitos", StringComparison.OrdinalIgnoreCase) || corpo.Contains("débitos", StringComparison.OrdinalIgnoreCase)))
                    {
                        // Verifica no email se esta dinheiro e devolve o total ao user
                        if (corpo.Contains("dinheiro"))
                        {
                            decimal total = hm.SomarValores(); // Soma os valores da tabela

                            // Pega o email do remetente que enviou
                            string remetente = mensagem.From.Mailboxes.First().Address;

                            // Envia resposta
                            EnviarResposta($"💰 O valor disponível no cartão é: {total}€",remetente, total);
                            // Marca como lido
                            inbox.AddFlags(uid, MessageFlags.Seen, true);
                        }

                        // Verifica no email se esta escrito gasnhos e adiciona ao db e devolve o total ao user
                        if (corpo != null && Regex.Match(corpo, @"\bganhos\s+(\d+(\.\d+)?)\b", RegexOptions.IgnoreCase).Success)
                        {
                            var match = Regex.Match(corpo, @"\bganhos\s+(\d+(\.\d+)?)\b", RegexOptions.IgnoreCase);
                            decimal ganhoDb = Convert.ToDecimal(match.Groups[1].Value);

                            hm.InserirHistorico("Ganhos", ganhoDb);
                            decimal total = hm.SomarValores(); // Soma os valores da tabela
                            string remetente = mensagem.From.Mailboxes.First().Address;

                            EnviarResposta($"Foram adicionados {ganhoDb}€.\n 💰 O valor disponível no cartão é: {total}€",remetente, total);
                            inbox.AddFlags(uid, MessageFlags.Seen, true);
                        }

                        // Verifica no email se esta escrito despesa ou debito e retira do db e devolve o total ao user
                        var match2 = Regex.Match(corpo ?? "", @"\b(debitos|despesas|débitos)\s+(\d+(\.\d+)?)\b", RegexOptions.IgnoreCase);
                        if (match2.Success)
                        {
                            decimal despesaDb = Convert.ToDecimal(match2.Groups[2].Value);

                            hm.InserirHistorico("Gastos", -(despesaDb));
                            decimal total = hm.SomarValores();
                            string remetente = mensagem.From.Mailboxes.First().Address;

                            EnviarResposta($"Foram gastos {despesaDb}€.\n 💰 O valor disponível no cartão é: {total}€", remetente, total);
                            inbox.AddFlags(uid, MessageFlags.Seen, true);
                        }


                    }
                }

                client.Disconnect(true);
            }
        }

        public void EnviarResposta(string resposta, string destinatario, decimal valorTotal)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress("Meu Bot", "joaotestecsharp@gmail.com")); // Usa o mesmo email autenticado
            mensagem.To.Add(new MailboxAddress("", destinatario));
            mensagem.Subject = "Saldo Atual";

            mensagem.Body = new TextPart("plain")
            {
                Text = resposta
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("joaotestecsharp@gmail.com", "alhm euco vodv jklm"); // App password

                client.Send(mensagem);
                client.Disconnect(true);
            }
        }
    }
}