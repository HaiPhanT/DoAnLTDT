# DoAnLTDT
Đồ án môn Lý thuyết đồ thị

# Các lưu ý: 
 - File input.txt phải đặt cùng thư mục với file Program.cs

# Cấu trúc kiểm tra và in kết quả:
 - Sử dụng hàm `PrintResult` để in kết quả
 - Đối với một ma trận kề, chúng ta sẽ sử dụng một kiểu dữ liệu Dictionary<Loại đồ thị, giá trị k(+ mảng k phân hoạch đối với đồ thị k phân hoạch)> dùng để in kết quả, gọi tên kiểu dữ liệu này là `graphTypeMapping`
 - Hiện tại chúng ta đã có danh sách các ma trận kề
 - Chúng ta sẽ duyện qua từng danh sách
 - Với mỗi danh sách, khởi tạo `graphTypeMapping` với các giá trị mặc định
 - Bây giờ, với mỗi câu hỏi, chúng ta sẽ tạo một hàm để check kiểu đồ thị
 - Hàm này cần nhận vào ma trận kề và `graphTypeMapping`
 - Sử dụng ma trận kề để kiểm tra loại đồ thị
 - Nếu loại đồ thị đang check có kết quả là `true`, thì thực hiện update giá trị tương ứng của loại đồ thị đó trong `graphTypeMapping`
 - Cần check tất cả 9 lại đồ thị và update kết quả (nếu check ra `true`) cho `graphTypeMapping`
 - Sau cùng sử dụng hàm `PrintResult` sẽ in ra kết quả như yêu cầu

@@Goodluck
