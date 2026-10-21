# Gözcü Görevi – Unity Projesi

## Proje Hakkında
Bu proje, Unity kullanılarak geliştirilen Gözcü Görevi ödev projesidir. Projede oyuncu hareketi, kamera kontrolü ve görüş alanına sahip bir gözcü sistemi geliştirilmiştir.

## Kullanılan Teknolojiler
- Unity
- C#
- Unity Input System
- CharacterController
- Git ve GitHub

## Oyuncu Kontrolleri

| Tuş | Görev |
|---|---|
| W, A, S, D | Oyuncuyu hareket ettirir. |
| Shift | Daha hızlı hareket etmeyi sağlar. |
| Fare | Bakış yönünü kontrol eder. |
| Space | Zıplamayı sağlar. |

## Gözcü Sistemi

Gözcü sistemi, oyuncunun konumunu ve görüş hattını değerlendirerek oyuncuyu algılar.

- **Görüş alanı:** Oyuncunun gözcünün belirlenen görüş açısı ve menzili içinde olup olmadığı kontrol edilir.
- **Engel kontrolü:** Ağaç gibi engeller oyuncu ile gözcü arasındaki görüşü kesebilir. Oyuncu ağacın arkasındayken gözcü tarafından algılanmaz.
- **Otomatik tarama:** Gözcü kendi çevresini belirlenen açısal sınırlar içinde sağa ve sola tarar.
- **Yükseklik sınırı:** Görüş kontrolünde yatay ve dikey açı sınırları dikkate alınır.
- **Oyuncuyu takip etme:** Kodda uygulanan takip davranışı, oyuncu algılandığında gözcünün tepkisini değiştirir.
- **Görsel geri bildirim:** Gözcünün algılama durumunu göstermek için kodda tanımlanan görsel geri bildirim kullanılır.

## Proje Klasörleri
- **Assets:** Oyun sahnesi, C# scriptleri ve kullanılan varlıklar.
- **Packages:** Unity paketleri.
- **ProjectSettings:** Proje ayarları.

## Projeyi Açma
1. Depoyu klonlayın veya bilgisayarınıza indirin.
2. Unity Hub üzerinden projeyi ekleyin.
3. Projeyi uygun Unity sürümüyle açın.
4. `Assets` klasöründeki ana sahneyi açarak çalıştırın.

## Geliştirici
AysheBerraYildiz

