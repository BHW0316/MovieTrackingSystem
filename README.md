# 電影追蹤系統

## 簡介

電影追蹤系統是一個用於追蹤電影上映時間和相關信息的應用程式。本專案提供 API，允許用戶檢索特定日期的電影上映時間和總覽信息。

## 安裝和執行

1. **安裝相依套件:**

   在專案的根目錄中執行以下命令：

```bash
dotnet restore
```
2. **運行應用程式:**

```bash
dotnet run
```
應用程式將運行在 localhost:defaultport。您可以在瀏覽器中訪問此 URL 以使用應用程式。

使用指南
API 端點
獲取所有電影上映時間:

```http
GET /api/movies/showtimes
```
獲取特定日期的電影總覽:

```http
GET /api/movies/overview/{date}
```
參數: date (日期格式: YYYY-MM-DD)

## 測試
運行測試:

使用以下命令運行測試：

```bash
dotnet test
```
## 文件結構
MovieTrackingSystem:
電影追蹤系統應用程式的程式碼。
MovieTrackingSystemTests:
電影追蹤系統應用程式的單元測試。
