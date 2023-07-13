namespace Projeto_Web;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.UseStaticFiles();
        app.MapGet("/", () => "Hello World!");
        app.MapGet("/cliente", () => "Cliente !!!!!");
        // app.MapGet("/produtos", () => "Produtos !!!!");
        app.MapGet("/produtos", (HttpContext contexto) =>
        {
            contexto.Response.Redirect("produtos.html", false)
    ;
        }
        );
        app.MapGet("/clientes", (string nome, string email) =>
        $"O nome do cliente escolhido é : {nome} \n O email é :{email}"
        );
        Pessoa p1 = new Pessoa() { id = 1, nome = "Ana" };
        // text/plain
        //app.MapGet("/fornecedores", () =>
        // $"O fornecedor é: {p1.id} - {p1.nome}"
        // );
        app.MapGet("/fornecedores", (HttpContext contexto) =>
        {
            string pagina = "<h1> Fornecedores </h1>";
            pagina = pagina + $"<h2> ID:{p1.id} - Nome : {p1.nome} </h2>";
            contexto.Response.WriteAsync(pagina);
        }
        );
        app.MapGet("/fornecedoresEnviarDados", (int id, string nome) =>
        {
            p1.id = id;
            p1.nome = nome;
            return "Dados Inseridos com sucesso";
        });
        app.MapGet("/api", (Func<object>)(() =>
        {
            return new
            {
                id = p1.id,
                nome = p1.nome
            };
        }
        ));
        Banco dba = new Banco();
        dba.carregarBanco();
        app.MapGet("/banco", (HttpContext contexto) =>
        {
            var valoresdalista = "";
            List<Pessoa> listaaux = dba.GetLista();
            foreach (Pessoa aux in listaaux)
            {
                valoresdalista = valoresdalista + $"<b> ID: </b>{aux.id} - <b> Nome:{aux.nome} </b> <br>";
            }
            //return valoresdalista;
            contexto.Response.WriteAsync(valoresdalista);
        });
        app.Run();
    }
}