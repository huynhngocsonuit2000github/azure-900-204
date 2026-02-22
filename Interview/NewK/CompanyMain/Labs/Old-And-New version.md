Dưới đây là **bức tranh full context**:

- Hệ thống CŨ đang chạy thế nào
- Hệ thống MỚI sẽ làm gì
- Process thay đổi ra sao
- End-to-end flow

Ngắn nhưng đủ ý.

---

# 🏚️ 1. DỰ ÁN CŨ (AS-IS)

## 🧩 Hệ thống

Có **2 system tách rời**

1️⃣ Solar
→ tạo **study spec (document)**

2️⃣ icollims desktop
→ **config database + chạy lab thật**

---

## 👥 Actors

- Sponsor → đưa requirement qua email / meeting
- Study setup user → viết spec + config DB
- QC user → check manual

---

## 🔄 Flow cũ

### Step 1 – Nhận requirement

Sponsor gửi:

- protocol
- list test
- visit
  ➡️ bằng email / file rời (không structured)

---

### Step 2 – Viết study spec trong Solar

User:

- đọc tài liệu
- nhập tay vào Solar
- tạo file spec

---

### Step 3 – Manual QC

Một người khác:

- mở file
- check bằng mắt

---

### Step 4 – Sponsor review

- gửi PDF qua email
- sửa → gửi lại

⛔ loop rất nhiều lần

---

### Step 5 – Double work 🔥

User:

- mở spec
- **nhập lại toàn bộ vào icollims desktop**

Ví dụ:

- visit
- test
- lab
- unit

---

### Step 6 – Go live

Lab:

- vào icollims
- nhập kết quả test

---

## ❌ Pain points

- UX tệ (desktop app)
- double data entry
- QC manual
- data sponsor không structured
- phải so sánh giữa 2 system

---

# 🚀 2. DỰ ÁN MỚI (TO-BE)

## 🎯 Mục tiêu

👉 1 web portal làm tất cả
👉 Auto QC
👉 Không nhập lại data
👉 Structured sponsor input

---

## 🧩 Vai trò hệ thống

### Portal (NEW – trung tâm)

Dùng để:

- intake protocol
- tạo study spec
- QC
- sponsor review
- config DB

### icollims

Chỉ còn:
➡️ **backend lab system**

---

# 🔄 FLOW MỚI (END-TO-END)

## 🥇 Step 1 – Protocol intake

Input:

- protocol
- proposal data
- icollims master data

AI (future) / user:
→ đổ data vào portal

---

## 🥈 Step 2 – Build study spec trên portal

Portal auto prefill:

- test master
- lab
- country
- sponsor data

User:

- mapping
- chỉnh sửa

---

## 🥉 Step 3 – Automated QC

User bấm Run QC → system check rule.

Không cần QC person riêng.

---

## 🏅 Step 4 – Sponsor review (trên portal)

Portal:

- generate PDF

Sponsor:

- comment trực tiếp

User:

- update realtime

Không email loop.

---

## 🏁 Step 5 – Database configuration

User config DB **ngay trên portal**

System:
→ translate
→ push xuống icollims DB

⛔ không nhập lại.

---

## 🚀 Step 6 – Go live

icollims:

- đã setup xong
- lab dùng để nhập result

---

# 🔥 SO SÁNH NGẮN

| AS-IS             | TO-BE       |
| ----------------- | ----------- |
| 2 system          | 1 portal    |
| Manual QC         | Auto QC     |
| Email sponsor     | Portal form |
| Double entry      | Push to DB  |
| Desktop app       | Web         |
| Unstructured data | Structured  |

---

# 🧠 NHỮNG GÌ DỰ ÁN MỚI PHẢI LÀM

## Core features

- Study setup portal (web)
- Data prefill engine
- QC rule engine
- Sponsor review workflow
- DB configuration UI
- Translation → icollims API / DB

## Integration

- icollims master data
- proposal data source
- future: AI protocol extraction

---

# ⭐ TÓM TẮT 1 CÂU

Hệ thống mới biến:

🧾 document-driven + manual + double entry
➡️ thành
🖥️ data-driven + automated + single source of truth

---

# Nếu bạn là dev

Có thể nghĩ theo module:

- Study management
- Spec data model
- QC engine
- Workflow / review
- Mapping → icollims schema
- Sync service

---

Nếu bạn muốn mình sẽ vẽ:

- sequence flow theo actor (User / Sponsor / System)
- hoặc kiến trúc backend → để nhìn dưới góc độ code.
