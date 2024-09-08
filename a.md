# Állatmenhely nyilvántartás
## Projekt adatok
|                   |                                               |                                                                   |
|-------------------|-----------------------------------------------|-------------------------------------------------------------------|
| **Téma**          | Backend és Fronted                            | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Témavezető**    | Méhes József                                  | A szoftvernek várhatóan nagyobb kitettsége ezen két tantárgy felé |
| **Cél** | Állatorvosi nyilvántartó rendszer létrehozása |                                                                   |
| **Fő platform** | Web |                                                                   |
| **Osztály**       | 13T-II                                        |                                                                   |
| **Csoporttagok**  | Dékány Csaba                                  | Boros Dániel                                                      |


## Tervezett funkciók
- Páciens (állat) nyilvántartás
  - Kezelési adatok
    - Kezelésekről külön megtekinthető adatok
    - Gyógyszerezési adatok
    - Gyógyszerezési ütemezés, amennyiben megfigyelés alatt van
      - (opcionális) Késő gyógyszerezés esetén értesítés az ügyeletes dolgozónak mobil appon keresztül
    - (opcionális) Amennyiben megfigyelés alatt van nyakkörvén lévő BAR kód felismerése mobiltelefonnal, ezzel könnyítve a páciens felismerését 
- Tulajdonosi nyilvántartás
  - Tulajdonos azonosítása lakcímkártya vagy cemélyigazolvány szerint
    - (opcionális) Mobil appon keresztül tulajdonos felismerése lakcímkártyán szereplő BAR kóddal
  - Időpontfoglalási felület a tulajdonosoknak
  - Tulajdonos kapcsolattartói adatainak nyilvántartása
  - Tulajdonoshoz tartozó páciensek megtekintése
  - Bejelentkező felület a tulajdonosoknak, időpontfoglaláshoz
    - Felhasználónév és jelszó alapú bejelentkezés a weblapon
    - (opcionális) Passwordless bejelentkezés mobil appon keresztül
- Alkalmazottak nyilvántartása
  - Bejelentkezés
    - Felhasználónév és jelszó alapú bejelentkezés a weblapon
    - (opcionális) Passwordless bejelentkezés mobil appon keresztül
  - Munkavállalói hozzáférés-ellenőrzési politika
    - Csoportos jogosultságok
    - Egyéni jogosultságok
    - Jogosultságok kezelése megfelelő hozzáférési szint esetén
- (opcionális) Számlázás
  - Sikeres kezelés esetén díjbekérő kiküldése ügyfél részére

## Architektúra
|                              |                 |                                       |
|------------------------------|-----------------|---------------------------------------|
| **Adatbázis**                | SQLite          | (tbd) Szakmaivizsga sajátosságai okán |
| **Adatbázis Csatoló**        | EntityFramework |                                       |
| **Backend API Szerver**      | ASP.NET Core    |                                       |
| **Frontend Framework**       | React           |                                       |
| **Frontend Design Rendszer** | Fluent UI 2     |                                       |
| **Mobil Keretrendszer**      | .NET MAUI       | (opcionális)                          |
