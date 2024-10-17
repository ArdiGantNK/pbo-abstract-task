using System;

abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public abstract void Serang(Robot target);

    public virtual void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}");
        Console.WriteLine($"Energi: {Energi}");
        Console.WriteLine($"Armor: {Armor}");
        Console.WriteLine($"Serangan: {Serangan}");
        Console.WriteLine("-------------------------");
    }

    public bool Mati()
    {
        return Energi <= 0;
    }
}

interface IKemampuan
{
    string Nama { get; }
    int Cooldown { get; }
    void Gunakan(Robot target);
}

class Perbaikan : IKemampuan
{
    public string Nama => "Perbaikan";
    public int Cooldown { get; private set; }

    public Perbaikan()
    {
        Cooldown = 3; // Set cooldown misal 3 giliran
    }

    public void Gunakan(Robot target)
    {
        target.Energi += 20; // Menambah energi
        Console.WriteLine($"{target.Nama} memulihkan energi sebesar 20!");
    }
}

class SeranganListrik : IKemampuan
{
    public string Nama => "Serangan Listrik";
    public int Cooldown { get; private set; }

    public SeranganListrik()
    {
        Cooldown = 2;
    }

    public void Gunakan(Robot target)
    {
        target.Energi -= 15; // Mengurangi energi
        Console.WriteLine($"{target.Nama} diserang dengan Serangan Listrik, energi berkurang 15!");
    }
}

class SeranganPlasma : IKemampuan
{
    public string Nama => "Serangan Plasma";
    public int Cooldown { get; private set; }

    public SeranganPlasma()
    {
        Cooldown = 5;
    }

    public void Gunakan(Robot target)
    {
        int damage = target.Armor > 0 ? 20 : 30; // Plasma lebih kuat jika armor
        target.Energi -= damage;
        Console.WriteLine($"{target.Nama} diserang dengan Serangan Plasma, energi berkurang {damage}!");
    }
}

class RobotBiasa : Robot
{
    private IKemampuan kemampuan;

    public RobotBiasa(string nama, int energi, int armor, int serangan, IKemampuan kemampuan)
        : base(nama, energi, armor, serangan)
    {
        this.kemampuan = kemampuan;
    }

    public override void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama}, energi berkurang {damage}!");
    }

    public void GunakanKemampuan(Robot target)
    {
        kemampuan.Gunakan(target);
    }
}

class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public override void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        target.Energi -= damage;
        Console.WriteLine($"{Nama} (Bos) menyerang {target.Nama}, energi berkurang {damage}!");
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - Armor;
        if (damage < 0) damage = 0;
        Energi -= damage;
        Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama}, energi berkurang {damage}!");
    }
}

class Permainan
{
    private RobotBiasa gundil;
    private RobotBiasa gundul;
    private BosRobot gundam;

    public Permainan()
    {
        gundil = new RobotBiasa("Gundil", 100, 20, 30, new SeranganListrik());
        gundul = new RobotBiasa("Gundul", 100, 25, 25, new SeranganPlasma());
        gundam = new BosRobot("Gundam", 100, 40, 50);
    }

    public void Mulai()
    {
        Console.WriteLine("SELAMAT DATANG WAHAI PETARUNG PERMAINAN AKAN SEGERA DIMULAI!");
        Console.WriteLine("INI ADALAH DAFTAR NAMA ROBOT BESERTA SPESIFIKASINYA!");

        while (gundil.Energi > 0 && gundul.Energi > 0 && !gundam.Mati())
        {
            gundil.CetakInformasi();
            gundul.CetakInformasi();
            gundam.CetakInformasi();

            Console.WriteLine("\nPilih robot untuk menyerang BOS:");
            Console.WriteLine("1. Gundil");
            Console.WriteLine("2. Gundul");
            string pilihanRobot = Console.ReadLine();

            RobotBiasa robotTerpilih = pilihanRobot == "1" ? gundil : gundul;

            Console.WriteLine("\nPilih aksi: ");
            Console.WriteLine("1. Langsung Serang Bos");
            Console.WriteLine("2. Gunakan Kemampuan Robot");
            string pilihanAksi = Console.ReadLine();

            if (pilihanAksi == "1")
            {
                robotTerpilih.Serang(gundam);
                if (!gundam.Mati()) gundam.Serang(robotTerpilih);
            }
            else if (pilihanAksi == "2")
            {
                robotTerpilih.GunakanKemampuan(gundam);
                if (!gundam.Mati()) gundam.Serang(robotTerpilih);
            }

            // Pemulihan energi di akhir setiap giliran
            gundil.Energi = Math.Min(100, gundil.Energi + 5);  // Pemulihan 5 energi
            gundul.Energi = Math.Min(100, gundul.Energi + 5);  // Pemulihan 5 energi

            Console.WriteLine("\nEnergi pulih sebesar 5 untuk Gundil dan Gundul.\n");

            if (gundam.Mati())
            {
                Console.WriteLine("Bos Gundam Kalah!");
                break;
            }
        }

        if (gundil.Energi <= 0 || gundul.Energi <= 0)
            Console.WriteLine("Robot Gundil atau Gundul Kalah!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Permainan game = new Permainan();
        game.Mulai();
    }
}
