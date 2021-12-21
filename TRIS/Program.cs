using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Console.Title = "TRIS!";
Console.CursorVisible = false;
Gioco gioco;
char ripeti = ' ';
//ripetizione del programma con invio o uscita con esc.
do
{
    Console.Clear();
    gioco = new Gioco();
    gioco.svolgimentoTurno();
    Console.Write("Premere ivio per ricominciare o esc per uscire: ");
    do
    {
        ripeti = Console.ReadKey(true).KeyChar;
    } while (ripeti != 13 && ripeti != 27);
} while (ripeti == 13);

public class Casella
{
    //Variabile per lo stato della casella.
    private byte statoCasella = 0;
    //Stringa per la costruzione a video dei simboli.
    private string[] simbolo { get; } = new string[6] { "\\ /", " X ", "/ \\", "/^\\", "|O|", "\\_/" };
    //StatoCasella accetta solo valori compresi tra 0 e 2
    public byte StatoCasella
    {
        get { return statoCasella; }
        set
        {
            if (value >= 0 && value <= 2) statoCasella = value;
            else throw new Exception("Valore casella non valido");
        }
    }

    public void riempiCasella(byte left = 0, byte top = 0)
    {
        //Costruzione a video della casella, se la casella è vuota allora non scrive nulla.
        if (statoCasella != 0)
        {
            Console.CursorTop = top;
            for (int i = 0; i < 3; i++)
            {
                Console.CursorLeft = left;
                Console.WriteLine(statoCasella == 1 ? simbolo[i] : simbolo[i + 3]);
            }
        }
    }
}

public class Tabella
{
    //Caselle.
    public Casella[] casella = new Casella[9];
    //Coordinate x e y tabella.
    private byte[] posizioneTabella = new byte[2] { 0, 0 };
    //PosizioneTabella rende publica posizioneTabella ma solo in lettura.
    public byte[] PosizioneTabella { get { return posizioneTabella; } }

    public Tabella()
    {
        //Inizzializzazione caselle
        for (int i = 0; i < casella.Length; i++) casella[i] = new Casella();
    }

    public void stampaTabella(byte left = 0, byte top = 0)
    {
        //Variabile di appoggio.
        byte appoggio = 0;
        //Coordinata x salvata;
        posizioneTabella[0] = left;
        //Coordinata y salvata;
        posizioneTabella[1] = top;

        //La coordinata y va richiamata solo una volta.
        Console.CursorTop = top;
        //Ciclo per costruire la tabella rigo per rigo.
        for (int i = 0; i < 11; i++)
        {
            //La coordinata x sposta il cursore verso destra in ogni nuovo rigo.
            Console.CursorLeft = left;
            //Scelta per capire quali caratteri stampare su ogni rigo.
            if (i == 3 || i == 7) Console.WriteLine("---+---+---");
            else if (i == 1 || i == 5 || i == 9)
            {
                //Calcolo numero casella nel rigo e nella colonna centrale di ogni casella.
                appoggio = (byte)(i / 4 * 3);
                Console.WriteLine($" {appoggio} | {appoggio + 1} | {appoggio + 2} ");
            }
            else Console.WriteLine("   |   |   ");
        }
    }

    public void riempiTabella(byte numeroCasella, byte turno)
    {
        //Riempimento della variabile statoCasella in base al turno.
        casella[numeroCasella].StatoCasella = turno;
        //Riempimento della casella giusta nella tabella graficamente tramite calcolo sulle coordinate.
        casella[numeroCasella].riempiCasella
        (
            (byte)(posizioneTabella[0] + (4 * (numeroCasella % 3))),
            (byte)(posizioneTabella[1] + (4 * (numeroCasella / 3)))
        );
    }
}

public class Gioco
{
    //Oggetto per rendere random il turno all'inizio.
    private Random rand = new Random();
    //Variabile turno.
    private byte turno = 0;
    //Tabella di gioco.
    private Tabella tabella = new Tabella();
    //Variabile per lettura inserimento utente.
    private byte lettura = 9;

    public Gioco()
    {
        //Turno random all'inizio con valori uguali allo statoCasella.
        turno = (byte)rand.Next(1, 3);
    }

    public void svolgimentoTurno()
    {
        //Creazione tabella e stampe iniziali.
        tabella.stampaTabella(18, 1);
        Console.Write("\nTurno di:\n\nInserire il numero della casella (da 0 a 8):");

        for (int i = 0; i < 9; i++)
        {
            //Aggiornamento grafico del turno.
            Console.SetCursorPosition(10, tabella.PosizioneTabella[1] + 12);
            Console.Write(turnoInChar());
            //Immissione della casella da parte dell'utente con relativi controlli.
            do
            {
                try
                {
                    lettura = (byte)char.GetNumericValue(Console.ReadKey(true).KeyChar);
                }
                catch (Exception) { lettura = 9; };

            } while (lettura < 0 || lettura > 8 || tabella.casella[lettura].StatoCasella != 0);

            //Riempimento della casella sia graficamente che nella variabile.
            tabella.riempiTabella(lettura, turno);
            /*Controllo vincitore e in caso uscita dal ciclo ma solo dopo che 
             * i giocatori hanno fatto due mosse a testa.*/
            if (i > 3 && controlloVincitore() == true) break;
            else if (i == 8) { turno = 0; break; }
            //Cambio giocatore.
            turno = (byte)(turno == 1 ? 2 : 1);
        }
        //Scritta vincitore.
        Console.CursorTop = tabella.PosizioneTabella[1] + 16;
        Console.WriteLine(turno == 0 ? "Pareggio!" : $"Ha vinto {turnoInChar()}!\n");
    }

    private bool controlloVincitore()
    {
        for (int i = 0; i < 3; i++)
        {
            if (
                (
                    //Controllo orizzontale.
                    tabella.casella[i * 3].StatoCasella != 0 &&
                    tabella.casella[i * 3].StatoCasella == tabella.casella[(i * 3) + 1].StatoCasella &&
                    tabella.casella[i * 3].StatoCasella == tabella.casella[(i * 3) + 2].StatoCasella
                ) || (
                    //Controllo verticale.
                    tabella.casella[i].StatoCasella != 0 &&
                    tabella.casella[i].StatoCasella == tabella.casella[i + 3].StatoCasella &&
                    tabella.casella[i].StatoCasella == tabella.casella[i + 6].StatoCasella
                ) || (
                    //Controllo obliquo.
                    i < 2 && tabella.casella[4].StatoCasella != 0 &&
                    tabella.casella[4].StatoCasella == tabella.casella[i * 2].StatoCasella &&
                    tabella.casella[4].StatoCasella == tabella.casella[8 - (i * 2)].StatoCasella
                )
                ) return true;
        }
        return false;
    }

    //Trasformazione del turno in carattere da stampare;
    private char turnoInChar()
    {
        return turno == 1 ? 'X' : 'O';
    }
}