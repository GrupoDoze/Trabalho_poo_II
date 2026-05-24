# Sistema de Gestão de Companhia Aérea
## Projecto 14 — C# (Visual Studio 2015 / .NET 4.6)

---

## Descrição

Sistema completo para gerir a operação de uma companhia aérea regional, incluindo aeronaves, voos, passageiros, reservas, bilhetes e check-in.

---

## Estrutura do Projecto

```
SistemaCompanhiaAerea/
│
├── SistemaCompanhiaAerea.sln          ← Solução Visual Studio 2015
│
└── SistemaCompanhiaAerea/
    ├── SistemaCompanhiaAerea.csproj   ← Projecto C# (.NET 4.6)
    ├── Program.cs                     ← Ponto de entrada + menu interactivo
    │
    ├── Enums/
    │   ├── ClasseVoo.cs              ← Económica, Executiva, PrimeiraClasse
    │   └── EstadoReserva.cs          ← Activa, Cancelada, CheckInRealizado, Embarcado
    │
    ├── Exceptions/
    │   ├── AssentoOcupadoException.cs
    │   ├── VooLotadoException.cs
    │   └── CheckInForaDoPrazoException.cs
    │
    ├── Models/
    │   ├── Assento.cs                ← Número, Classe, Ocupado
    │   ├── Aeronave.cs               ← Matrícula, Tipo, Capacidade, Assentos
    │   ├── Voo.cs                    ← Número, Rota, Horário, Aeronave, Reservas
    │   ├── Passageiro.cs             ← Id, Nome, Passaporte, Email, Telefone
    │   ├── Bilhete.cs                ← NumeroBilhete, Classe, Assento, Preço
    │   └── Reserva.cs                ← Id, Passageiro, Voo, Estado, Bilhete
    │
    └── Services/
        └── GestorCompanhia.cs        ← Coordenação central de todas as operações
```

---

## Classes e Relações (conforme Diagrama UML)

| Classe           | Responsabilidade                                              |
|-----------------|---------------------------------------------------------------|
| `Aeronave`       | Frota: tipo, capacidade, lista de assentos por classe         |
| `Voo`            | Rota, horário, aeronave associada, reservas e preços          |
| `Passageiro`     | Dados pessoais do passageiro                                  |
| `Assento`        | Número, classe e estado de ocupação                           |
| `Bilhete`        | Compõe a Reserva (gerado automaticamente com a reserva)       |
| `Reserva`        | Associa Passageiro ↔ Voo; contém o Bilhete por composição     |
| `GestorCompanhia`| Gestor central: cadastros, voos, reservas, check-in           |

### Relações POO implementadas

- **Composição**: `Reserva` contém `Bilhete` (o Bilhete só existe dentro da Reserva)
- **Associação**: `Voo` associa `Aeronave`; `Reserva` associa `Voo` e `Passageiro`
- **Enums**: `ClasseVoo`, `EstadoReserva`
- **Colecções genéricas**: `List<T>`, `Dictionary<TKey, TValue>`
- **LINQ**: filtragens, ordenações e projecções em todas as listagens

---

## Excepções Personalizadas

| Excepção                        | Quando é lançada                                              |
|---------------------------------|---------------------------------------------------------------|
| `AssentoOcupadoException`       | Tentativa de reservar assento já ocupado                      |
| `VooLotadoException`            | Voo sem lugares disponíveis na classe pretendida              |
| `CheckInForaDoPrazoException`   | Check-in fora da janela permitida (24h a 1h antes da partida) |

---

## Funcionalidades Implementadas

1. **Cadastro de aeronaves** — tipo, matrícula, capacidade por classe (Económica/Executiva/1.ª Classe)
2. **Criação e gestão de voos** — rota, horário, aeronave, preços por classe
3. **Reserva de bilhetes** — selecção de classe, assento automático, preço com antecedência
4. **Cancelamento de reservas** — liberta o assento, invalida o bilhete
5. **Cálculo de preço por antecedência**:
   - +60 dias → 30% desconto
   - 30-59 dias → 15% desconto
   - 14-29 dias → preço normal
   - 7-13 dias → 15% extra
   - <7 dias → 30% extra
6. **Processo de check-in** — janela de 24h a 1h antes da partida
7. **Confirmação de embarque**
8. **Listagem de passageiros por voo**
9. **Relatório geral** da companhia (aeronaves, voos, reservas, receita)
10. **Impressão de bilhete** com todos os detalhes

---

## Como Abrir no Visual Studio 2015

1. Abrir o ficheiro `SistemaCompanhiaAerea.sln` com Visual Studio 2015
2. Compilar: **Build → Build Solution** (F6)
3. Executar: **Debug → Start Without Debugging** (Ctrl+F5)

> **Nota:** O projecto usa .NET Framework 4.6 e C# 6, compatível com Visual Studio 2015.

---

## Dados de Demonstração (carregados automaticamente)

Ao iniciar, o sistema carrega:

| Item         | Quantidade |
|-------------|-----------|
| Aeronaves   | 3 (A320, 737-800, E190) |
| Voos        | 4 (LIS↔OPO, LIS↔FAO, OPO→LIS, LIS→PDL) |
| Passageiros | 5          |
| Reservas    | 5          |

---

## Autor
Projecto 14 — Programação Orientada a Objectos em C#
