## Use Cases
### Use Case 1 Kunde Registrierung

**Beschreibung:** Als Kunde möchte ich meine Identität von der Bibliothek bestätigen lassen, damit ich mich später einloggen kann.

**Ablauf:**
1. Kunde gibt die Daten ein.
2. Die Administration überprüft die Identität und bestätigt sie.

**Akzeptanzkriterien**
1. Der Kunde ist bestätigt.

---

### Use Case 2 Kunde Login

**Beschreibung:** Als Kunde möchte ich mich selbstständig anmelden, damit ich bequem die Bibliotheksinfrastruktur nutzen kann.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde gibt die Daten ein.
    - Name
    - Passwort
    - einmalige Authentifizierungs-ID
3. Weiterleitung zur Startseite.

**Akzeptanzkriterien**
1. Der Kunde kann sich mit der gegebenen Authentifizierungs-ID im System anmelden.

---

### Use Case 3 Artikelverwaltung: Rückgabe

**Beschreibung:** Als Kunde möchte ich bestimmte Artikel zurückgeben können, um die Bibliotheksinfrastruktur weiter nutzen zu können.

**Ablauf:**
1. Kunde wählt „Ausgeliehen“.
2. Kunde kann:
    - Spezifisches Buch auswählen → zurückgeben

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch zurückgeben.

---

### Use Case 3.1 Artikelverwaltung: Ausleihe

**Beschreibung:** Als Kunde möchte ich bestimmte Artikel ausleihen können, um die Bibliotheksinfrastruktur nutzen zu können.

**Ablauf:**
1. Kunde wählt „Ausgeliehen“.
2. Kunde kann:
    - Spezifisches Buch auswählen → ausleihen

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch ausleihen.

---

### Use Case 3.2 Artikelverwaltung: Reservierung

**Beschreibung:** Als Kunde möchte ich bestimmte Artikel reservieren können, um die Bibliotheksinfrastruktur nutzen zu können.

**Ablauf:**
1. Kunde wählt „Ausgeliehen“.
2. Kunde kann:
    - Spezifisches Buch auswählen → reservieren

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch reservieren.

---

### Use Case 4 Suchverwaltung

**Beschreibung:** Als Kunde möchte ich bequem durch alle Bücher suchen können, damit ich effizient interessante Artikel finde.

**Ablauf:**
1. Kunde wählt „Katalog“. 
2. Kunde kann:
    - Nach Genre suchen → spezifisches Buch auswählen.
    - Nach Titel suchen → spezifisches Buch auswählen.
    - Nach Jahr suchen → spezifisches Buch auswählen.

**Akzeptanzkriterien**
1. Der Kunde kann Bücher anhand spezifischer Artikelinformationen, z. B. Genre, suchen.

---

### Use Case 5 Kontoverwaltung

**Beschreibung:** Als Kunde möchte ich ein Verwaltungspanel haben, damit ich mein Konto selbstständig verwalten kann.

**Ablauf:**
1. Kunde wählt „Profil“.
2. Kunde kann:
    - Das Konto löschen.
    - Die Kontoinformationen aktualisieren.    

**Akzeptanzkriterien**
1. Der Kunde kann sein eigenes Konto schliessen.
2. Der Kunde kann seine Kontoinformationen aktualisieren.

---

### Use Case 6 Artikelverwaltung: Ausleihhistorie

**Beschreibung:** Als Kunde möchte ich meine Ausleihhistorie übersichtlich einsehen können, um zu wissen, wie viele Bücher ich aktuell ausgeliehen habe.

**Ablauf:**
1. Kunde wählt „Ausgeliehen“.
2. Kunde kann:
    - Die gesamte Ausleihhistorie einsehen.
    - Nach Genre suchen → Ausleihhistorie einsehen.

**Akzeptanzkriterien**
1. Die UI zeigt eine Liste mit aktiven Ausleihen.
2. Die UI hat Filteroptionen für die Artikelsuche nach Genre.