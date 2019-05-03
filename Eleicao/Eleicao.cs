using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eleicao
{
    class Eleicao
    {
        private Candidato[] candidatos = new Candidato[3];
        private int qtdEleitores = 0;
        private int votosBranco = 0;
        private int votosNulo = 0;

        static void Main(string[] args)
        {
            Eleicao eleicao = new Eleicao();

            //cria os candidatos da eleicao
            for (int i = 0; i < 3; i++)
            {
                Console.Clear();
                eleicao.candidatos[i] = eleicao.createCandidato();
            }

            //verifica quantos eleitores participaram da eleicao
            Console.Clear();
            Console.WriteLine("Digite o número de eleitores que votaram na eleicao:");
            eleicao.qtdEleitores = int.Parse(Console.ReadLine());

            eleicao.processoEleitoral(eleicao, eleicao.candidatos);
        }

        //cria um novo candidato de acordo com os dados fornecidos
        public Candidato createCandidato()
        {
            String nome;
            int numero;
            String partido;
            int idade;

            Console.WriteLine("Digite o nome do candidato:");
            nome = Console.ReadLine();

            Console.WriteLine("Digite o número do candidato:");
            numero = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o partido do candidato:");
            partido = Console.ReadLine();

            Console.WriteLine("Digite a idade do candidato:");
            idade = int.Parse(Console.ReadLine());

            return new Candidato(nome, numero, partido, idade);
        }

        //le os votos de cada eleitor e atribui ao candidato correspondente
        public void atribuirVotos(Eleicao eleicao, Candidato[] candidatos, int voto)
        {
            int contNulo = 0;

            if (voto == 0)
                eleicao.votosBranco++;
            else
            {
                foreach (Candidato candidato in candidatos)
                {
                    if (voto == candidato.numero)
                        candidato.qtdVotos++;
                    else
                        contNulo++;
                }
            }
            //se nenhum candidato recebeu votos
            if (contNulo == candidatos.Length)
                eleicao.votosNulo++;
        }

        //verifica quem venceu ou se houve empate
        public Candidato[] verificaVencedor(Candidato[] candidatos)
        {
            int maior = 0;
            int maiorIdade = 0;
            String vencedor = "";
            int contVencedor = 0;

            foreach (Candidato candidato in candidatos)
            {
                if (candidato.qtdVotos > maior)
                {
                    maior = candidato.qtdVotos;
                    vencedor = candidato.nome;
                }
            }
            foreach (Candidato candidato in candidatos)
                if (candidato.qtdVotos == maior)
                    contVencedor++;

            //se tiver mais de um vencedor
            //verificacao por idade
            if (contVencedor > 1)
            {
                contVencedor = 0;
                foreach (Candidato candidato in candidatos)
                {
                    if (candidato.qtdVotos == maior)
                    {
                        if (candidato.getIdade() > maiorIdade)
                        {
                            maiorIdade = candidato.getIdade();
                            vencedor = candidato.nome;
                        }
                    }
                }

                foreach (Candidato candidato in candidatos)
                    if (candidato.qtdVotos == maior)
                        if (candidato.getIdade() == maiorIdade)
                            contVencedor++;
            }

            Candidato[] candidatosV = new Candidato[contVencedor];

            //se tiver apenas um vencedor
            if (contVencedor == 1)
            {
                foreach (Candidato candidato in candidatos)
                    //verifica quem venceu de acordo com o nome
                    if (candidato.nome == vencedor)
                    {
                        candidatosV[0] = candidato;
                        return candidatosV;
                    }
            }
            //se houve mais de um vencedor ( empate )
            else if (contVencedor > 1)
                for (int i = 0; i < candidatos.Length; i++)
                    if (candidatos[i].qtdVotos == maior)
                        if (candidatos[i].getIdade() == maiorIdade)
                            //adiciona ao array temporario os candidatos empatados
                            candidatosV[i] = candidatos[i];

            //retorna um array com os candidatos empatados
            return candidatosV;
        }

        //mostra os resultados da eleição
        public void mostraResultado(Candidato[] vencedor, Eleicao eleicao, Candidato[] candidatos)
        {
            if (vencedor.Length == 1)
            {
                Console.WriteLine("Candidato vencedor: " + vencedor[0].nome);
                Console.WriteLine();
            }
            foreach (Candidato candidato in candidatos)
            {
                Console.WriteLine("Nome: " + candidato.nome);
                Console.WriteLine("Partido: " + candidato.partido);
                Console.WriteLine("Quantidade de Votos: " + candidato.qtdVotos);
                Console.WriteLine();
            }
            Console.WriteLine("Votos nulos: " + eleicao.votosNulo);
            Console.WriteLine("Votos em branco: " + eleicao.votosBranco);
            Console.ReadKey();
        }

        //funcao onde ocorre a eleição
        public void processoEleitoral(Eleicao eleicao, Candidato[] candidatos)
        {
            int voto = 0;

            //entrada dos votos
            for (int i = 1; i <= eleicao.qtdEleitores; i++)
            {
                Console.Clear();
                foreach (Candidato candidato in candidatos)
                    Console.WriteLine("Candidato: " + candidato.nome + " | Numero: " + candidato.numero);

                Console.WriteLine("Digite o voto do " + i + " eleitor");
                voto = int.Parse(Console.ReadLine());

                eleicao.atribuirVotos(eleicao, candidatos, voto);
            }
            Console.Clear();

            //recebe um array que contem um ou mais candidatos de acordo com a verificação 
            Candidato[] vencedor = eleicao.verificaVencedor(candidatos);
            int candidatosNaoNulos = 0;

            //verificacao de vitoria ou empate
            foreach (Candidato candidato in vencedor)
            {
                if (candidato != null)
                    candidatosNaoNulos++;
            }
            if (candidatosNaoNulos > 1)
            {
                Console.WriteLine("!!!EMPATE!!!");
                Console.WriteLine("Nova eleicao deve ser realizada");
                Console.WriteLine();
                eleicao.mostraResultado(vencedor, eleicao, candidatos);
                Console.WriteLine();
                Console.WriteLine("Clique em qualquer tecla para votar novamente");
                Console.ReadKey();
                eleicao.votosBranco = 0;
                eleicao.votosNulo = 0;

                //redefine os votos dos candidatos para 0
                foreach (Candidato candidato in vencedor)
                    candidato.qtdVotos = 0;

                //a função é executada novamente apenas com os candidatos que ficaram em empate
                //trata-se de uma função recursiva, o que substitui um loop
                eleicao.processoEleitoral(eleicao, vencedor);
            }
            else
            {
                //se houve um vencedor então os resultados são mostrados
                eleicao.mostraResultado(vencedor, eleicao, candidatos);
            }
        }
    }

    //classe que representa o candidato
    class Candidato
    {
        public String nome;
        public int numero;
        public String partido;
        private int idade;
        public int qtdVotos;

        public Candidato(String nome, int numero, String partido, int idade)
        {
            this.nome = nome;
            this.numero = numero;
            this.partido = partido;
            this.idade = idade;
            this.qtdVotos = 0;
        }

        public int getIdade()
        {
            return this.idade;
        }
    }
}
