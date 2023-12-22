using System;
using System.Text;
using System.Threading;

class Program
{
    const int ChieuRongKhung = 90;
    const int ChieuCaoKhung = 26;
    const int ChieuRongCuaSo = 70;

    static int thoiGianRoi;
    static int diem = 0;
    static int diemCaoNhat = 0;
    static bool dangBatNhacNen = true;
    static int soLanSpam = 4;// Số chữ rơi cùng 1 lúc 

    static char[] kyTu = new char[10];
    static int[,] viTriKyTu = new int[10, 2];
    static void GoToXY(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }
    static void ThuocTinhConTro(bool nhinThay)
    {
        Console.CursorVisible = nhinThay;
    }
    static void ThuocTinhCuaSo()
    {
        Console.SetWindowSize(ChieuRongKhung + 2, ChieuCaoKhung + 1);
        Console.SetBufferSize(ChieuRongKhung + 2, ChieuCaoKhung + 1);
    }
    static void NhacNen()
    {
        int C = 261;
        int D = 294;
        int E = 329;
        int F = 349;
        int G = 392;
        int A = 440;
        int B = 494;

        // Định nghĩa thời gian cho mỗi nốt nhạc 
        int quarterNote = 500;
        int halfNote = 1000;

        while (true)
        {
            while (dangBatNhacNen)
            {
                // Chơi nhạc
                Console.Beep(C, quarterNote);
                Console.Beep(D, quarterNote);
                Console.Beep(E, quarterNote);
                Console.Beep(F, halfNote);
                if (!dangBatNhacNen)
                    break;
                Console.Beep(G, quarterNote);
                Console.Beep(A, quarterNote);
                Console.Beep(B, halfNote);

                Thread.Sleep(150);
            }
        }
    }
    static void VeBang()
    {
        Console.ForegroundColor = ConsoleColor.Red;

        for (int i = 0; i < ChieuRongKhung; i++)
        {
            Console.Write("±");
        }

        for (int i = 0; i < ChieuRongKhung+1 ; i++) // Cộng 1 để lấp đầy chỗ trống
        {
            GoToXY(i, ChieuCaoKhung);
            Console.Write("±");
        }

        for (int i = 0; i < ChieuCaoKhung; i++)
        {
            GoToXY(0, i);
            Console.Write("±");
            GoToXY(ChieuRongKhung, i);
            Console.Write("±");
        }

        for (int i = 0; i < ChieuCaoKhung; ++i)
        {
            GoToXY(ChieuRongCuaSo, i);
            Console.Write("±");
        }

        Console.ResetColor();
    }
    static void TaoKyTuNgauNhien(int dong)
    {
        Random r = new Random(Guid.NewGuid().GetHashCode());// Tạo random
        kyTu[dong] = (char)(65 + r.Next(0, 26));//Chữ in hoa từ A đến Z
        // Đảm bảo chữ ko rơi ngay khung
        viTriKyTu[dong, 0] = 2 + r.Next(0, ChieuRongCuaSo - 2);
        viTriKyTu[dong, 1] = 1;
    }
    static void VeKyTu(int dong)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        GoToXY(viTriKyTu[dong, 0], viTriKyTu[dong, 1]);
        Console.Write(kyTu[dong]);
        Console.ResetColor();
    }
    static void XoaKyTu(int dong)
    {
        GoToXY(viTriKyTu[dong, 0], viTriKyTu[dong, 1]);
        Console.Write(" ");
    }
    static void DatLaiKyTu(int dong)
    {
        XoaKyTu(dong);
        TaoKyTuNgauNhien(dong);
    }
    static void Diem()
    {
        GoToXY(ChieuRongCuaSo + 7, 5);
        Console.WriteLine($"ĐIỂM: " + diem + "\n");

        GoToXY(ChieuRongCuaSo + 2, 7);
        Console.WriteLine($"ĐIỂM CAO NHẤT: " + diemCaoNhat + "\n");
    }
    static void VeNoiDungBenPhai()
    {
        GoToXY(ChieuRongCuaSo + 5, 2);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("GAME ĐÁNH CHỮ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        GoToXY(ChieuRongCuaSo + 6, 4);
        Console.Write("----------");

        GoToXY(ChieuRongCuaSo + 6, 6);
        Console.Write("----------");

        GoToXY(18, 5);
        Console.Write("                      ");
        Console.ResetColor();

    }
    static bool CoNhapDung(char key, char kyTu)
    {
        if (key == kyTu || key - 32 == kyTu)
        {
            return true;
        }
        return false;
    }
    static void XuLyChoi()
    {
        Console.Clear();
        VeBang();
        Diem();
        VeNoiDungBenPhai();

        for (int i = 0; i < soLanSpam; i++)
        {
            TaoKyTuNgauNhien(i);
        }

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char key = keyInfo.KeyChar;
                for (int i = 0; i < soLanSpam; i++)
                {
                    // Kiểm tra xem có nhập đúng không?
                    if (CoNhapDung(key, kyTu[i]))
                    {
                        DatLaiKyTu(i);// Gồm xóa và vẽ ký tự
                        diem++;
                        Diem();
                    }
                }
                // Tăng độ khó trò chơi sau 10 điểm, thời gian rơi > 70 để đảm bảo không bị lỗi
                if (diem % 10 == 0 && thoiGianRoi > 70)
                {
                    thoiGianRoi -= 15;
                }
                // Nhấn Esc thì thoát
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
            }

            // In ký tự đã định sẵn
            for (int i = 0; i < soLanSpam; i++)
            {
                VeKyTu(i);
            }

            // Delay tạo hiệu ứng chữ rơi
            Thread.Sleep(thoiGianRoi);

            for (int i = 0; i < soLanSpam; i++)
            {
                XoaKyTu(i);// Xóa để tạo mô phỏng rơi
                viTriKyTu[i, 1]++;// Tăng hàng lên 1 đơn vị, đảm bảo sẽ rơi xuống chứ ko giữ nguyên vị trí
                if (viTriKyTu[i, 1] >= ChieuCaoKhung)
                {
                    GameOver();
                    return;
                }
            }
        }
    }
    static void GameOver()
    {
        Console.Clear();
        Console.WriteLine("\t\t-------------------------------");
        Console.WriteLine("\t\t------ TRÒ CHƠI KẾT THÚC ------");
        Console.WriteLine("\t\t-------------------------------");
        Console.WriteLine($"\t\tĐiểm của bạn là: {diem}");
        if (diem > diemCaoNhat)
        {
            diemCaoNhat = diem;
            Console.WriteLine("\t\tXin chúc mừng bạn đã đạt kỷ lục mới\n");
        }
        Console.WriteLine("\t\tNhấn phím Enter để quay lại bảng chọn hoặc nhấn r để chơi lại");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\t\tĐIỂM CAO NHẤT: {diemCaoNhat}");
        Console.ResetColor();

        bool nenThoat = false;

        while (!nenThoat)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    nenThoat = true;
                    break;
                default:
                    if (keyInfo.KeyChar == 'r' || keyInfo.KeyChar == 'R') // Chơi lại
                    {
                        ChoiTroChoi();
                        nenThoat = true;
                    }
                    break;
            }
        }
    }
    static void HuongDan()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t\tHƯỚNG DẪN");
        Console.WriteLine("Bạn sẽ thấy chữ rơi phía bên trái màn hình");
        Console.WriteLine("Hãy nhấn phím tương thích với các kí tự trên màn hình");
        Console.WriteLine("Nếu kí tự nào chạm đáy bạn sẽ thua");
        Console.WriteLine("NHỚ TẮT BỘ GÕ TIẾNG VIỆT TRƯỚC KHI CHƠI");
        Console.WriteLine("\nNhấn ESC để thoát hướng dẫn");

        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
            HuongDan();
            return;
        }
    }
    static void VeMenu()
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.Clear();
        Console.Write("~");
        Console.SetCursorPosition(10, 5);
        Console.WriteLine("--------------------------");
        Console.SetCursorPosition(10, 6);
        Console.WriteLine("|      GAME ĐÁNH CHỮ     |");
        Console.SetCursorPosition(10, 7);
        Console.WriteLine("--------------------------");
        Console.SetCursorPosition(10, 9); Console.ResetColor();

        Console.WriteLine("1. Bắt đầu chơi");
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("2. Hướng dẫn");
        Console.SetCursorPosition(10, 11);
        Console.WriteLine("3. Âm thanh");
        Console.SetCursorPosition(10, 12);
        Console.WriteLine("Nhấn 4 hoặc ESC để thoát");
        Console.SetCursorPosition(10, 13);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Nhập số để lựa chọn: ");

        Console.ResetColor();
    }
    static void XuLyLuaChon(char luaChon)
    {
        switch (luaChon)
        {
            case '1':
                ChoiTroChoi();
                break;
            case '2':
                HuongDan();
                break;
            case '3':
                QuanLyNhac();
                break;
            case '4':
            case '\u001b':
                Environment.Exit(0);
                break;
        }
    }
    static void ChoiTroChoi()
    {
        diem = 0;
        thoiGianRoi = 800;
        XuLyChoi();
    }
    static void QuanLyNhac()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        GoToXY(10, 5);
        Console.WriteLine("1. Tắt nhạc nền");
        GoToXY(10, 6);
        Console.WriteLine("2. Bật nhạc nền");
        GoToXY(10, 8);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Nhấn ESC để thoát");
        Console.ResetColor();
        char luaChonNhac = Console.ReadKey().KeyChar;
        switch (luaChonNhac)
        {
            case '1':
                if (dangBatNhacNen)
                {
                    dangBatNhacNen = false;
                }
                return;
            case '2':
                if (!dangBatNhacNen)
                {
                    dangBatNhacNen = true;
                }
                return;
            case '\u001b': // Là nhấn esc
                return;
            default: QuanLyNhac(); return;
        }
    }
    static void LuaChon()
    {
        Thread nhac = new Thread(NhacNen);
        if (dangBatNhacNen)
            nhac.Start();
        do
        {
            VeMenu();
            char luaChon = Console.ReadKey().KeyChar; 
            Console.Clear();
            XuLyLuaChon(luaChon);
        } while (true);
    }
    static void main(string[] args)
    {
        ThuocTinhConTro(false);
        ThuocTinhCuaSo();
        Console.OutputEncoding = UTF8Encoding.UTF8;// Ghi tiếng việt dùng thư viện Text
        LuaChon();
    }
}