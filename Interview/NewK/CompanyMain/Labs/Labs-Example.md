# Step 1 – Sponsor gửi protocol

Protocol nói:

- Day 1: test A, B
- Day 30: test A
- Nếu A > 300 → alert
- Chạy ở VN & Thai

# Step 2 – Tạo study spec (trong portal/Solar)

User cấu hình:

- Visit schedule
- Test cho từng visit
- Panic rule
- Country/site
  -> Xuất file → sponsor duyệt → ký ✅

# Step 3 – Setup database

System dùng study spec để:

- Tạo visit trong DB
- Gán test
- Set rule

(Hiện tại: làm tay trong icolims)

# Step 4 – QC

Check tự động hoặc manual:
So sánh:

| Study spec   | Database         |
| ------------ | ---------------- |
| Visit        | có đủ chưa       |
| Test         | đúng visit chưa  |
| Rule         | đúng ngưỡng chưa |
| Country/site | đúng chưa        |

# Step 5 – Go-live

- Site bắt đầu lấy mẫu bệnh nhân
- Lab chạy test
- Kết quả đổ vào system
- System tự áp rule (ví dụ >300 → alert)

======================
**_Ví dụ FULL flow theo hệ thống mới – Study Setup Portal_**

# Step 1 – Nhận protocol

- Upload protocol lên portal
- (Tương lai AI đọc → tự extract: visit, test, rule…)

# Step 2 – Sponsor điền form

Portal gửi form chuẩn cho sponsor:

- Chọn country
- Chọn test cần làm
- Chọn visit schedule
  → Sponsor submit
  -> Portal auto prefill study spec draft

# Step 3 – Study setup cấu hình trên web

User chỉ làm trong 1 màn hình web:

- Review visit
- Add/remove test
- Set panic value
- Chọn site

💡 Data test code / country code → tự lấy từ DB cũ → không cần nhập tay.

# Step 4 – QC tự động (bấm nút Run QC)

System tự check, ví dụ:

- Visit có nhưng chưa gán test
- Test chưa có panic rule
- Country chưa có lab mapping

→ Hiện list lỗi để sửa.

✔ Không cần người khác ngồi so manual.

# Step 5 – Xuất study spec & sponsor ký

- Generate PDF
- Gửi sponsor review & sign

# Step 6 – Go-live (KHÔNG cần config lại ở system khác)

Ngay khi ký:

Portal tự push config xuống database lab, ví dụ:

- Tạo visit
- Gán test
- Set rule

🚫 Không còn bước nhập tay trong icolims.

# Step 7 – Run study

- Site làm test
- Lab trả kết quả
- System áp rule & hiển thị data

# Flow in one line

Study auto tạo
→ Sponsor chọn info qua form
→ User chọn visit + test + rule
→ Run QC
→ Sponsor ký
→ System auto setup DB
→ Lab chạy & có kết quả
