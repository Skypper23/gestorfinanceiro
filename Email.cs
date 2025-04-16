using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit;
using MimeKit;
using System.Linq;

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

                    if (corpo != null && corpo.Contains("dinheiro"))
                    {
                        decimal total = hm.SomarValores(); // Soma os valores da tabela

                        // Pega o email do remetente que enviou
                        string remetente = mensagem.From.Mailboxes.First().Address;

                        // Envia resposta
                        EnviarResposta(remetente, total);
                        // Marca como lido
                        inbox.AddFlags(uid, MessageFlags.Seen, true);
                    }
                }

                client.Disconnect(true);
            }
        }

        public void EnviarResposta(string destinatario, decimal valorTotal)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress("Meu Bot", "joaotestecsharp@gmail.com")); // Usa o mesmo email autenticado
            mensagem.To.Add(new MailboxAddress("", destinatario));
            mensagem.Subject = "Saldo Atual";

            mensagem.Body = new TextPart("plain")
            {
                Text = $"💰 O valor disponível no cartão é: {valorTotal}€"
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