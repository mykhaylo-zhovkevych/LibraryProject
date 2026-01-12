## Ziel der Software

- **Ziel:** Eine Bibliotheks-App, die sowohl für Administration als auch für Kunden nutzbar ist.
- **Plattformen:** Desktop-App (Avalonia, MVVM) und Web-App (Blazor).
- **Datenhaltung:** lokal gespeichert, ggf. persistent (z.B. Datei oder SQLite).
- **Kernlogik**: Alle Bibliotheksfunktionen in einem **Core-Projekt** definiert und unabhängig von UI.

---
## Voraussetzungen

1. Die bestehende Codebasis für dieses Projekt übernehmen und drauf aufbauen.
2. Das System muss zwei Interfaces haben: eines für die Bibliotheksadministration und eines für Kunden.
3. Die Daten können lokal gespeichert werden; optional soll eine persistente Datenspeicherung umgesetzt werden.
4. Der Code muss statisch typisiert sein und den Clean-Code-Konventionen folgen.

---

## Benutzergruppen & Rollen

Admin
- Bücher hinzufügen, bearbeiten, löschen
- Benutzer verwalten (registrieren, löschen)
- Ausleihhistorie überwachen

Kunde
- Bücher ausleihen, zurückgeben, reservieren
- Bücher suchen (mit Erweiterungen)
- Eigenes Konto verwalten (Konto schliessen)
- Ausleihhistorie einsehen (mit Erweiterungen)

---
## Use Cases

### Use Case 1: Buchverwaltung

**Beschreibung:** Als Admin möchte ich neue Bücher hinzufügen sowie bestehende Bücher bearbeiten oder löschen, damit die Infrastruktur der Bibliothek einwandfrei ist.

**Ablauf:**
1. Admin meldet sich an.
2. Admin geht zu "Verwaltung". 
3. Admin wählt "Benutzer Verwaltung".
4. Admin kann:
   - Buch hinzufügen → Formular ausfüllen → speichern
   - Buch bearbeiten → Buch auswählen → Änderungen speichern
   - Buch löschen → Buch auswählen → löschen
4. System aktualisiert lokale Daten (In-Memory(File) + optional persistent)

**Akzeptanzkriterien**
1. Der Admin kann ein oder mehrere Bücher ins System einfügen.
2. Der Admin kann bestehende Bücher bearbeiten.
3. Der Admin kann ein oder mehrere Bücher löschen.

### Use Case 2: Benutzerverwaltung

**Beschreibung:** Als Admin möchte ich Kundenkonten und Kunde verwalten können, um in Sonderfällen bestimmte Aktionen durchzuführen, z.B. sperren.

**Ablauf:**
1. Admin meldet sich an.
2. Admin geht zu "Verwaltung". 
3. Admin wählt "Benutzerverwaltung".
4. Admin kann:
   - Kunde manuelle hinzufügen → Formular ausfüllen→ speichern
   - Kunde bearbeiten → Änderungen speichern
   - Kunde auswählen → löschen
4. System aktualisiert lokale Daten

**Akzeptanzkriterien**
1. Der Admin kann bestehende Kunden aus dem System löschen.
2. Der Admin kann neue Kunden ins System einfügen.
3. Der Admin kann bestehende Kundeninformationen bearbeiten.

## Use Case 3: Richtlinie Verwaltung

**Beschreibung** Als Admin möchte ich eine Überblick von alle Richtlinien haben, damit die Infrastruktur Beziehungen zwischen Objekten und Entitäten korrekte definieren kann. 

**Ablauf:**
1. Admin meldet sich an.
2. Admin geht zu "Verwaltung". 
3. Admin wählt "Richtlinien Verwaltung".
4. Admin kann:
	- Richtlinie manuelle hinzufügen → Formular ausfüllen → speichern 
	- Richtlinie aktualisieren →  Änderungen speichern
	- Richtlinie auswählen → löschen
5. System aktualisiert lokale Daten

**Akzeptanzkriterien**
5. Der Admin kann neue Policy ins System einfügen.
6. Der Admin kann bestehende Policy aus dem System löschen.
7. Der Admin kann bestehende Policy Information aktualisieren. 

### Use Case 4: Ausleihvorgänge überwachen

**Beschreibung** Als Admin möchte ich die vollständige Ausleihhistorie eines Kunden einsehen können, um einen Überblick zu erhalten.

**Ablauf:**
1. Admin meldet sich an.
2. Admin geht zu "Verwaltung". 
3. Admin wählt "Benutzerverwaltung".
4. Admin kann:
   - Kunde auswählen → Kunde Ausleihhistorie durchblättern.

**Akzeptanzkriterien**
1. Der Admin kann die gesamte Ausleihhistorie eines spezifischen Kunden einsehen.


### Use Case 5 Kunde Registrierung

**Beschreibung** Als Kunde möchte eine Kunde Identität von Bibliothek haben, damit ich mich später einloggen kann.

**Ablauf:**
1. Kunde öffnet Programm.
2. Kunde macht eine Registrierung .
3. Kunde gibt die Daten ein.
	- Name 
	- Password
	- Adresse
	- Email
4. Admin überprüft die Identität und bestätigt.
5. System aktualisiert lokale Daten .

**Akzeptanzkriterien**
1. Der Kunde bekommt eine Bestätigung vom System (z.B. per Email).

### Use Case 6 Kunde Login

**Beschreibung** Als Kunde möchte ich mich selbständig anmelden, damit ich bequem Bibliothek Infrastruktur nutzen kann.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde gibt die Daten ein.
	- Name 
	- Password
	- einmalige authentifizierung ID
3. Verifizierung und Authentifizierung von Daten.
4. Weiterleitung zu Startseite.

**Akzeptanzkriterien**
1. Der Kunde kann mit gegebenen authentifizierung ID sich im System einmelden.

### Use Case 7 Artikeln Verwaltung: Zurückgabe  

**Beschreibung** Als Kunde möchte ich bestimmte Artikeln zurückgeben können, um die Bibliotheksinfrastruktur nutzen zu können.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Ausgeliehen".
3. Kunde kann:
	- Spezifische Buch auswählen → zurückgeben
4. System aktualisiert lokale Daten.

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch zurückgeben.


### Use Case 7.1  Artikeln Verwaltung: Ausleihe 

**Beschreibung** Als Kunde möchte ich bestimmte Artikeln ausleihen können, um die Bibliotheksinfrastruktur nutzen zu können.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Ausgeliehen".
3. Kunde kann:
	- Spezifische Buch auswählen → ausleihen
4. System überprüft Kunde Bedienung.
5. System aktualisiert lokale Daten.

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch ausleihen.


### Use Case 7.2  Artikeln Verwaltung: Reservierung  

**Beschreibung** Als Kunde möchte ich bestimmte Artikeln reservieren können, um die Bibliotheksinfrastruktur nutzen zu können.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Ausgeliehen".
3. Kunde kann:
	- Spezifische Buch auswählen → reservieren
4. System überprüft Kunde Bedienung.
5. System aktualisiert lokale Daten.

**Akzeptanzkriterien**
1. Der Kunde kann ein spezifisches Buch reservieren.

### Use Case 8 Suchen Verwaltung

**Beschreibung** Als Kunde möchte ich bequem durch alle Bücher suchen können, damit ich effizient interessante Artikel finde.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Katalog".
3. Kunde kann:
   - Nach Genre suchen → spezifische Buch auswählen.
   - Nach Title suchen → spezifische Buch auswählen.
   - Nach Jahr suchen → spezifische Buch auswählen.

**Akzeptanzkriterien**
1. Der Kunde kann Bücher anhand spezifischer Artikelinformationen z.B. Genre suchen.

### Use Case 9 Kontoverwaltung

**Beschreibung** Als Kunde möchte ich ein Verwaltungspanel haben, damit ich mein Konto selbstständig steuern kann.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Profile".
3. Kunde kann:
   - Die Konto löschen.
   - Die Konto Information aktualisieren.
4. System aktualisiert lokale Daten.

**Akzeptanzkriterien**
1. Der Kunde kann sein eigenes Konto schliessen.
2. Der Kunde kann seine Kundeninformationen aktualisieren.

### Use Case 10 Ausleihhistorie

**Beschreibung** Als Kunde möchte ich meine Ausleihhistorie übersichtlich einsehen können, um zu wissen, wie viele Bücher ich aktuell ausgeliehen habe.

**Ablauf:**
1. Kunde meldet sich an.
2. Kunde wählt "Profile".
3. Kunde kann:
   - Die ganze Ausleihhistorie einsehen.
   - Genre suchen → Ausleihhistorie einsehen.
   - Title suchen → Ausleihhistorie einsehen.

**Akzeptanzkriterien**
1. Der Kunde kann die gesamte Ausleihhistorie einsehen.
2. Der Kunde kann die aktive und inaktive Ausleihhistorie auswählen.
3. Der Kunde kann spezifische Artikel aus der Ausleihhistorie entfernen (optional).

---

## Arbeitsplan und Arbeitspakete mit Aufwandsschätzungen

| Datum                   | Zeit | Arbeitspaket                        | Erwartete Aufgabe                           | Erledigte Aufgabe |
| ----------------------- | ---- | ----------------------------------- | ------------------------------------------- | ----------------- |
| 05.01.2026 / 06.01.2026 |      | Vorbereitung und Konzept erstellung | Das fertig Konzept                          |                   |
| 07.01.2026 / 09.01.2026 |      | Projektinitialisierung              | Die Core Projekt und allfällige Remastering |                   |
| 12.01.2026 / 16.01.2026 |      | Desktop App Implementierung         |                                             |                   |
| 19.01.2026 / 23.01.2026 |      | Web App Implementierung             |                                             |                   |
| 26.01.2026 / 30.01.2026 |      | Refactoring                         |                                             |                   |

---
## Architekturkonzept

### Clean Architecture

[![Design Application using Clean Architecture - DEV Community](https://media2.dev.to/dynamic/image/width=1000,height=420,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Forahufsvjymejio3beu7.png)

**Beschreibung** Das gesamte Projekt ist in vier Sichten (Layer) unterteilt. Jede Sicht hat eine klar definierte Verantwortung und darf nicht direkt mit anderen Sichten kommunizieren, sondern nur horizontal über definierte Schnittstellen.
Dies ermöglicht eine bessere Testbarkeit, Wartbarkeit und Erweiterbarkeit der Anwendung.

1. Domain Layer
2. Application Layer
3. Presentation Layer
4. Infrastructure Layer

**Vor -und Nachteile**

1. Klare Trennung der Verantwortlichkeiten
   Jede Schicht hat eine eindeutige Aufgabe: Die Domain ist für Geschäftslogik und Entitäten zuständig. Der Application Layer steuert Use Cases und den Ablauf der Anwendung. Die Infrastructure kümmert sich um Datenhaltung, Datenbanken und externe Services. Die Presentation ist für die Benutzeroberfläche verantwortlich.
2. Einfache Testbarkeit
   Unit-Tests können einfach umgesetzt werden, da die Infrastructure-Schicht durch Mock- oder Fake Implementierungen ersetzt werden kann.
3. Wartbarkeit
   Neue Features können hinzugefügt werden, ohne andere Layer stark zu verändern.
4. Saubere Abhängigkeiten
   Die Domain kennt die Infrastructure nicht, und die Application kennt die UI nicht.
   Dadurch entsteht eine lose Kopplung zwischen den einzelnen Teilen des Codes.

5. Setup Komplexität
   Der initiale Aufbau benötigt mehr Zeit, insbesondere bei kleinen oder mittelgrossen Projekten.
6. Event Hadling / Cross-Cutting Concerns
   Querschnittsthemen wie Logging, Validierung oder Security sind komplexer umzusetzen.

**Warum diese Architektur wurden ausgewählt?**
Ich habe diese Architektur ausgewählt, weil das passt für die kleine bis mitlernen Projekten die eine gemeinsamen Geschäftslogik haben müssen und die Trennung zwischen Sichten ermöglichen flexiblem Erwartbarkeit ohne die Kernlogik zu verändern müssen.

---
## Datenkonzept

![[Pasted diagram.png]]

**Wie und Wo werden die Daten gespeichert?**
Die Daten werden an zwei Orten gespeichert. Die erste Implementierung erfolgt REM und die zweite erfolgt mit SQLite.



## Testprotokoll

| Modul | Aufgabe          | Verantwortung Aufgabe | Ergebnis      | Beteiligt an der Ergebniserstellung |
| ----- | ---------------- | --------------------- | ------------- | ----------------------------------- |
| Tests | Test durchführen | Testverantwortlicher  | Testprotokoll | Tester                              |
|       |                  |                       |               |                                     |