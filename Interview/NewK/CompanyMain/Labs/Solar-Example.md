# 1. Giả sử sponsor có protocol cho study thuốc tiểu đường:

Protocol nói:

- Visit 1 (Day 1): lấy máu → test Glucose, HbA1c
- Visit 2 (Day 30): test Glucose
- Nếu Glucose > 300 → cảnh báo khẩn
- Study chạy ở: Việt Nam, Thái Lan

# 2. Study spec trong Solar sẽ cấu hình lại thành:

## Lịch visit

- V1 – Day 1
- V2 – Day 30

## Test cho từng visit

- V1 → Glucose, HbA1c
- V2 → Glucose

## Rule

- Glucose > 300 → panic value → gửi alert

## Phạm vi áp dụng

- Site thuộc Vietnam & Thailand

## Sau đó xuất PDF → sponsor xem → OK → ký.

==========
Khi đã ký duyệt xong nghĩa là:

- Cấu hình đã được chốt (approved)
- Lab/system bắt đầu chạy study thật theo cấu hình đó
- Khi bệnh nhân vào:
  - hệ thống tự biết đang ở visit nào
  - cần làm test gì
  - có cảnh báo gì không
