# Akıllı Prompt Backend

<div align="center">
        <img src="./logos/yazilim_academy_logo_320.png" alt="Yazılım Academy Logo" height="100"/>
        &nbsp;&nbsp;
        <img src="./logos/logo_240x202.png" alt="Akıllı Prompt Logo" height="100"/>
</div>
<br><br>

Merhaba, Türkiye'nin geliştirici topluluğu! Şu an üzerinde çalıştığımız "Akıllı Prompt" projesine hoşgeldiniz. Bu proje, ChatGPT, Claude, Gemini, MidJourney gibi farklı yapay zeka modellerinin kullanıcıları için bir "prompt" kütüphanesi ve topluluğu yaratmayı amaçlıyor. Bizim amacımız, bu alandaki geliştiricileri ve meraklıları bir araya getirerek, paylaşım ve dayanışma ruhunu canlı tutmak. Projemizi açık kaynak hale getirerek Türkiye geliştirici topluluğuna katkıda bulunmak istiyoruz. 🤝

## Proje Hakkında

Bu proje, Minimum Viable Product (MVP) anlayışıyla geliştirilmekte olup, öğren, inşa et ve tekrar et döngüsüyle ilerlemeyi hedefliyoruz. Kullanıcı geri bildirimlerine dayalı olarak sürekli iyileştirme ve büyüme odaklı bir yaklaşımla hareket ediyoruz.

Akıllı Prompt, kullanıcıların farklı yapay zeka modelleriyle kullanabileceği etkili "prompt"lar oluşturabileceği bir SaaS uygulamasıdır. Bu uygulama, hem ücretli bir hizmet sunacak hem de arka uç ve ön uç tarafından açık kaynak kodu sunarak geliştiricilere açık olacak. Bu repo, projenin .NET 9 ve ASP.NET Core Web API kullanılarak geliştirilmiş arka uç (backend) bileşenini içermektedir.

## Mimari

Proje "Clean Architecture" prensiplerine uygun olarak tasarlanmıştır ve aşağıdaki katmanlardan oluşmaktadır:

- **Domain**: Uygulamanın temel iş mantığını ve kurallarını barındıran katmandır.
- **Persistence**: Veri erişimi ve veritabanı işlemlerini yürüten katmandır. Entity Framework Core kullanılmaktadır.
- **Web API**: RESTful API'leri barındıran ve kullanıcı taleplerine cevap veren katmandır.

## Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1. **Depoyu Klonlayın**

   ```sh
   git clone https://github.com/YazilimAcademy/AkilliPrompt.git
   cd AkilliPrompt
   ```

2. **Bağımlılıkları Yükleyin**

   - Proje .NET 9 kullanıyor, bu nedenle .NET 9 SDK'sını kurduğunuzdan emin olun.
   - Entity Framework Core, proje bağımlılıklarından biridir. Aşağıdaki komutu kullanarak veritabanı güncellemelerini uygulayabilirsiniz:
     ```sh
     dotnet ef database update
     ```

3. **Uygulamayı Çalıştırın**

   ```sh
   dotnet run --project AkilliPrompt.WebApi
   ```

## Katkıda Bulunma

Bu projenin gelişiminde katkıda bulunmak isteyen herkesi bekliyoruz! Katkıda bulunmak için:

1. Bir "issue" oluşturarak hataları bildirin veya önerilerde bulunun.
2. Yeni bir özellik eklemek ya da hata düzeltmek istiyorsanız, bir "pull request" oluşturabilirsiniz.
3. Katkı sağlarken, lütfen "Clean Code" prensiplerine ve projenin kod standartlarına uygun şekilde kod yazmaya dikkat edin.

## Lisans

Bu proje, MIT Lisansı altında sunulmuştur. Daha fazla bilgi için `LICENSE` dosyasına göz atabilirsiniz.

Hep birlikte Türkiye'deki yazılım geliştirici topluluğunu daha da ileri taşıyalım!

## Topluluk ve İletişim

Akıllı Prompt projesinin geliştirilmesi ve büyütülmesi sürecinde siz de yer almak ister misiniz? Gelin, birlikte öğrenip gelişelim! 🤝

- **Discord**: Topluluğumuza katılın ve diğer geliştiricilerle sohbet edin. [Discord Bağlantısı](https://discord.gg/yazilimacademy)
- **Yazılım Academy Web**: Daha fazla bilgi ve kaynak için [Yazılım Academy Web Sitesi](https://yazilim.academy/)
- **YouTube**: Eğitim videoları ve duyurular için [YouTube Kanalımız](https://www.youtube.com/@yazilimacademy)




### Oricin ve Yazılım Academy’deki ekip arkadaşlarımıza çok teşekkür ederiz. 👇

<a href="https://github.com/oricintechnologies"><img width="60px" alt="AltuDev" src="https://github.com/oricintechnologies.png"/></a>
<a href="https://github.com/iparzival0"><img width="60px" alt="AltuDev" src="https://github.com/iparzival0.png"/></a>

## Teşekkürler
Projemize gösterdiğiniz ilgi ve desteğiniz için çok teşekkür ederiz. Birlikte daha büyük bir topluluk oluşturabilir ve daha faydalı projeler geliştirebiliriz. 🙏
