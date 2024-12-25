Merhaba,

Projeyi zip içerisinden çıkarttıktan sonra açmadan önce VS2022 içerisinde veri ve.Net paketinin kurulu olduğundan emin olmalıyız. Sonrasında Market1.sln uzantısına tıklayarak projeyi başlatabiliriz.

Hemen ardından bu paketler yüklü ise projeyi başlatmak istediğimizde proje gizlilik hatası verecektir. 

Başlat seçeneğinden sonra karşılaşılacak tek hata, projenin farklı bir cihazda
indirilmesinden kaynaklı web dosyalarının koruma altına alınmasıdır.

Bu sorununun çözümü program çalıştırıldığında hata veren .resx uzantılı
dosyaların adlarına dikkat ederek projenin bulunduğu Market1 klasörü içerisindeki .resx uzantılı dosyaları
koruma altından kaldırmak olacaktır. Kaldırma işlemi sırasıyla hata veren .resx uzantılı dosyalara sağ tıklayarak
genel:güvenlik/engellemeyi kaldır seçeneği aktif et/uygula/tamam
şeklindedir. Bu aşamaların ardından proje sorunsuz bir şekilde açılmaktadır.

Proje başlatıldıktan sonra giriş ekranında kullanıcı adı ve şifreye admin
olarak giriş yapmayı denedikten sonra program bir tepki vermiyorsa
projeyi durdurup program sınıfı açılmalı. Application.Run(new fLogin());
kodu Application.Run(new fAyarlar()); olarak düzenlenip proje başlatılmalı.
Açılan ayarlar sayfasından kullanıcı kaydı yapılmalı, tüm yetkiler kullanıcıya
verilmeli ve kaydettikten sonra proje durdurulmalı. Sonrasında program sınıfı
sayfası tekrar açılmalı ve kod Application.Run(new fLogin()); olarak eski
haline getirilip proje başlatılmalı. Şimdi gelen giriş ekranından az önce
oluşturduğumuz kullanıcı bilgileri ile projeyi istediğimiz kullanabiliriz.

Projenin tüm kodlarını yorum satırları oluşturarak hangi kod bloğunun hangi fonksiyonu yerine getirdiğini açıklayarak
oluşturmaya dikkat ettim.
