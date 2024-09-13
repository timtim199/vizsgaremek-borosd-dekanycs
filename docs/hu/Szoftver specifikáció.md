# Szoftverspecifikáció


## Projekt adatok
|                  |                                               |                                                                   |
|------------------|-----------------------------------------------|-------------------------------------------------------------------|
| **Projekt neve** | Állatorovosi nyilvántartó rendszer            |                                                                   |
| **Téma**         | Backend és Fronted                            | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Témavezető**   | Méhes József                                  | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Fő platform**  | Web                                           |                                                                   |
| **Osztály**      | 13T-II                                        |                                                                   |
| **Csoporttagok** | Dékány Csaba                                  | Boros Dániel                                                      |

## Tartalomjegyzék
<!-- TOC -->
* [Szoftverspecifikáció](#szoftverspecifikáció)
  * [Projekt adatok](#projekt-adatok)
  * [Tartalomjegyzék](#tartalomjegyzék)
  * [1. Bevezetés](#1-bevezetés)
  * [2. Funkcionális követelmények](#2-funkcionális-követelmények)
  * [3. Külső interfész követelményei](#3-külső-interfész-követelményei)
  * [4. Nem funkcionális követelmények](#4-nem-funkcionális-követelmények)
    * [4.1 Biztonság](#41-biztonság)
      * [4.1.1 Adatbiztonság](#411-adatbiztonság)
      * [4.1.2 Hitelesítés és jogosultságkezelés](#412-hitelesítés-és-jogosultságkezelés)
      * [4.1.3 Audit naplók](#413-audit-naplók)
      * [4.1.4 Mobil biztonság](#414-mobil-biztonság)
    * [4.2 Kapacitás](#42-kapacitás)
      * [4.2.1 Tárolási terv](#421-tárolási-terv)
      * [4.2.2 Felhő/helyi szerver infrastruktúra](#422-felhőhelyi-szerver-infrastruktúra)
    * [4.3 Kompatibilizás](#43-kompatibilizás)
      * [4.3.1 Operációs rendszerek támogatása](#431-operációs-rendszerek-támogatása)
    * [4.4  Megbízhatóság és rendelkezésre állás](#44--megbízhatóság-és-rendelkezésre-állás)
      * [4.4.1 Uptime követelmények](#441-uptime-követelmények)
      * [4.4.2 Hibatűrés](#442-hibatűrés)
      * [4.4.3 Adatintegritás](#443-adatintegritás)
    * [4.5  Karbantarthatóság](#45--karbantarthatóság)
      * [4.5.1 Folyamatos integráció/telepítés (CI/CD)](#451-folyamatos-integrációtelepítés-cicd)
      * [4.5.2 Dokumentáció](#452-dokumentáció)
    * [4.6  Használhatóság](#46--használhatóság)
      * [4.6.1 Intuitív UI/UX kialakítás](#461-intuitív-uiux-kialakítás)
      * [4.6.2 Reszponzív dizájn](#462-reszponzív-dizájn)
      * [4.6.3 Hozzáférhetőség](#463-hozzáférhetőség)
      * [4.6.4 Oktatási és segédanyagok](#464-oktatási-és-segédanyagok)
  * [5. Definiciók és rövidítések](#5-definiciók-és-rövidítések)
  * [6. További dokumentumok](#6-további-dokumentumok)
<!-- TOC -->

## 1. Bevezetés

## 2. Funkcionális követelmények

## 3. Külső interfész követelményei

## 4. Nem funkcionális követelmények
### 4.1 Biztonság

#### 4.1.1 Adatbiztonság
Minden érzékeny információt, kifejezetten ami a felhasználó azonosítására alkalmas titkosított formában kell eltárolni az adatbázisban

#### 4.1.2 Hitelesítés és jogosultságkezelés
![](https://treewebsolutions.com/uploads/article/63/what-is-role-based-access-control-rbac_fliclzP1bLsQm9-5.jpg)
Biztonságos bejelentkezési módszerek (felhasználónév/jelszó) használata kétfaktoros hitelesítési lehetőséggel és jelszó nélküli mobilos belépési opcióval. Szerepkör-alapú hozzáférés-vezérlés (RBAC) megvalósítása a felhasználók számára a megfelelő adathozzáférés érdekében. 
> ⚠️ ️️️Passwordless bejelentkezés és a kétfaktoros hitelesítés opcionális 

#### 4.1.3 Audit naplók
Nyilvántartást kell vezetni a rendszerhozzáférésekről és az adatok módosításáról, különösen az orvosi és tulajdonosi adatok tekintetében.

#### 4.1.4 Mobil biztonság
Biztosítani kell a mobil alkalmazások és a háttérrendszer közötti biztonságos kommunikációt, beleértve az adatok titkosítását és az eszközök biztonságát.

### 4.2 Kapacitás
#### 4.2.1 Tárolási terv
A rendszernek képesnek kell lennie az orvosi feljegyzések, tulajdonosi információk és kezelési adatok tárolására több éven keresztül. A növekvő tárolási igényeket a páciensek számának növekedése és az adatok megőrzési politikája alapján kell tervezni.
> ⚠️ A tárolt adat mértéke nem haladhatja meg az Amazon RDS és S3 Free Tier limitjeit
#### 4.2.2 Felhő/helyi szerver infrastruktúra
A szoftver futtatását lehetővé kell tenni mind lokális, és mind felhőbeli (Amazon AWS) környezetben.
> ✔️ Kivételt képeznek ez alól az olyan esehetőségek, ahol a lokális környezetben a feltételek nem állnak rendelkezésre (pl.: internet hozzáférés)

> ⛔ A szoftver kódbázisának módosítása nélkül kell ezt lehetővé tenni.

> ⚠️ A szoftver futtatására felhasznált erőforrás nem haladhatja meg az Amazon AWS Free Tier limitációit.

### 4.3 Kompatibilizás
#### 4.3.1 Operációs rendszerek támogatása
Biztosítani kell a kompatibilitást a Windows és az Android alapú operációs rendszerek támogatását.
> ⚠️ A mobilalkalmazás elkészítése opcionális.

> ⛔ A webes alkalmazás reszponzivitását biztosítani kell minden eszközön.

### 4.4  Megbízhatóság és rendelkezésre állás
#### 4.4.1 Uptime követelmények
A rendszernek 99,9%-os rendelkezésre állással kell működnie, különösen a mobil alkalmazások esetében, amelyek sürgős feladatokat (pl. gyógyszerészeti értesítések) látnak el. A tervezett karbantartást munkaidőn kívül kell ütemezni.

#### 4.4.2 Hibatűrés
Hibatűrő mechanizmusokat kell bevezetni a magas rendelkezésre állás érdekében, és a kritikus hibák idejét maximum 1 órára kell korlátozni normál használat mellett.

#### 4.4.3 Adatintegritás
Biztosítani kell, hogy rendszerösszeomlás esetén se vesszenek el adatok, és valós idejű mentési mechanizmusok működjenek a kritikus adatoknál.

### 4.5  Karbantarthatóság
#### 4.5.1 Folyamatos integráció/telepítés (CI/CD)
A rendszernek automatizált CI/CD "pipeline"-nal kell rendelkeznie, amely gyors frissítéseket és biztonsági javításokat tesz lehetővé.
#### 4.5.2 Dokumentáció
Teljes körű belső dokumentációt kell biztosítani a fejlesztők számára, részletes API-referenciával a külső integrációkhoz.

### 4.6  Használhatóság
#### 4.6.1 Intuitív UI/UX kialakítás
A webes és mobil alkalmazásoknak felhasználóbarát, intuitív felülettel kell rendelkezniük mind a klinikai személyzet, mind a páciensek tulajdonosai számára.
#### 4.6.2 Reszponzív dizájn
A rendszer webes felületének reszponzívnak kell lennie, könnyen alkalmazkodva a különböző képernyőméretekhez, beleértve a tableteket és okostelefonokat is.
#### 4.6.3 Hozzáférhetőség
A webes hozzáférhetőségi szabványok (pl. WCAG) követése biztosítja, hogy a szoftver használható legyen fogyatékkal élők számára is.
#### 4.6.4 Oktatási és segédanyagok
Biztosítani kell egy segédszekciót oktatóanyagokkal, GYIK-kel és esetleg egy chatbotot az azonnali segítségnyújtáshoz a felhasználóknak.

## 5. Definiciók és rövidítések
|                      |                                                                                                                                                                                                                                               |
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **SRS**              | Software Requirment Specification                                                                                                                                                                                                             |
| **Páciens**          | A szoftver adatbázisában szereplő állat                                                                                                                                                                                                       |
| **Tulajdonos**       | A szoftver adatbázisában szereplő állat tulajdonosa                                                                                                                                                                                           |
| **Munkavállaló**     | A szoftver adatbázisában szereplő "employee" szerepkörrel rendelkező felhasználó                                                                                                                                                              |
| **RBAC**             | Role-based access control                                                                                                                                                                                                                     |
| Amazon RDS           | Amazon Realtional Database                                                                                                                                                                                                                    |
| Amazon AWS Free Tier | [Link](https://aws.amazon.com/free/?all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=tier%2312monthsfree%7Ctier%23always-free&awsf.Free%20Tier%20Categories=categories%23databases) |
| Amazon S3            | Amazon Object Storage                                                                                                                                                                                                                         |


## 6. További dokumentumok

