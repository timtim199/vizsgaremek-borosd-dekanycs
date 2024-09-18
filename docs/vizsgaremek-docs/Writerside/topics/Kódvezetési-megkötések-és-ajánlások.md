# Kódvezetési megkötések és ajánlások

## Bevezetés

A következő irányelvek célja, hogy segítsenek a fejlesztőknek tiszta, könnyen olvasható és karbantartható kódot írni. A tiszta kód javítja a fejlesztési folyamatot, és könnyebbé teszi a hibajavítást és a bővítést.

> Minden le nem fedett esetben a  [Microsoft irányelvei](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/) illetve [Robert C. Martin](https://gist.github.com/wojteklu/73c6914cc446146b8b533c0988cf8d29) ajánlásai irányadóak.
{style="note"}
## Általános Irányelvek

### Elnevezés
- **Beszédes nevek**: Használj érthető, beszédes neveket változóknak, függvényeknek és osztályoknak.
- **Konzisztencia**: Tartsd meg a konzisztens nevezéktant a teljes kódbázisban.
- **Kerüld a rövidítéseket**: Kerüld a rövidítéseket, kivéve ha azok széles körben elfogadottak (pl. `min`, `max`).
- **PascalCasing használata**: minden publikus tagra, típusra, namespace-re, stb.
- **camelCasing használata**: Minden paraméterre
-

### Kommentek
- **Magyarázó kommentek**: Csak akkor használj kommenteket, ha a kód nem egyértelmű. A jól megírt kód önmagáért beszél.
- **Dokumentációs kommentek**: Használj dokumentációs kommenteket a függvények és osztályok fölött, hogy leírják azok célját és használatát.

### Formázás
- **Behúzás**: Használj következetes behúzást (pl. 4 szóköz vagy 1 tabulátor).
- **Sorhossz**: Tartsd meg a sorhosszt 80-120 karakter között.
- **Üres sorok**: Használj üres sorokat a kód logikai részeinek elválasztására.

## Strukturális Irányelvek

### Függvények
- **Kis függvények**: Írj kis, egy dolgot végző függvényeket. Ha egy függvény túl hosszú, bontsd kisebb részekre.
- **Egy szintű absztrakció**: Egy függvény csak egy szintű absztrakcióval dolgozzon.

### Osztályok
- **Kis osztályok**: Az osztályok legyenek kicsik, és egyértelműen meghatározott feladatkörük legyen.
- **Egység elve**: Egy osztály csak egy dolgot csináljon, de azt jól.

## Kódolási Irányelvek

### Hibakezelés
- **Kivételkezelés**: Használj kivételkezelést a hibák kezelésére. Ne hagyj üres `catch` blokkokat.
- **Tiszta állapot**: Biztosítsd, hogy a program tiszta állapotba kerül hibakezelés után.

### Tesztelhetőség
- **Egység tesztek**: Írj egység teszteket minden függvényhez és osztályhoz.
- **Mock objektumok**: Használj mock objektumokat a teszteléshez, hogy ne függj a külső rendszerektől.

## Verziókezelés

### Commit üzenetek
- **Beszédes commit üzenetek**: Írj világos és érthető commit üzeneteket, amelyek leírják a változtatásokat.
- **Kis commitok**: Gyakran commitolj, és kis, jól elkülöníthető változtatásokat végezz.

## Példák

### Rossz példa
```
public void process() {
    // feldolgozás
    for (int i = 0; i < 10; i++) {
        // valami logika
    }
}
```

### Jó példa
```
/**
 * Adatok feldolgozása.
 */
public void processData() {
    for (int index = 0; index < MAX_ENTRIES; index++) {
        processEntry(index);
    }
}

/**
 * Egy bejegyzés feldolgozása.
 * 
 * @param index A bejegyzés indexe.
 */
private void processEntry(int index) {
    // konkrét logika itt
}
```