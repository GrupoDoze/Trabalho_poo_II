<div align="center">

# ✈️ Sistema de Gestão de Companhia Aérea

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_4.6-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio_2015-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)
![POO](https://img.shields.io/badge/POO_II-0A1628?style=for-the-badge&logo=academia&logoColor=white)

> **Projecto académico** desenvolvido no âmbito da disciplina de Programação Orientada a Objectos II  
> Instituto Superior Politécnico — Departamento de Engenharia Informática — UÍGE / 2026

</div>

---

## 📋 Índice

- [Sobre o Projecto](#-sobre-o-projecto)
- [Funcionalidades](#-funcionalidades)
- [Estrutura do Projecto](#-estrutura-do-projecto)
- [Classes e Relações UML](#-classes-e-relações-uml)
- [Excepções Personalizadas](#-excepções-personalizadas)
- [Preço Dinâmico](#-cálculo-de-preço-dinâmico)
- [Como Executar](#-como-executar)
- [Dados de Demonstração](#-dados-de-demonstração)
- [Autores](#-autores)

---

## 🛫 Sobre o Projecto

Sistema completo para gerir a operação de uma companhia aérea regional, desenvolvido em **C#** com o paradigma de **Programação Orientada a Objectos**.

O sistema cobre todo o fluxo operacional: desde o cadastro de aeronaves e passageiros, criação de voos, efectuação de reservas com emissão de bilhetes, até ao processo de check-in e confirmação de embarque.

### 🎯 Conceitos de POO Aplicados

| Conceito | Aplicação no Projecto |
|---|---|
| **Encapsulamento** | Todos os atributos com `get` público e `private set` |
| **Composição** | `Reserva` ◆── `Bilhete` (o Bilhete só existe dentro da Reserva) |
| **Associação** | `Reserva` ──▶ `Passageiro` e `Voo`; `Voo` ──▶ `Aeronave` |
| **Enums** | `ClasseVoo` e `EstadoReserva` |
| **Colecções Genéricas** | `List<T>` e `Dictionary<TKey, TValue>` |
| **LINQ** | Filtragens, ordenações, projecções e cálculo de receita |
| **Excepções** | 3 excepções personalizadas para erros operacionais |

---

## ✅ Funcionalidades

- 🛩️ **Gestão de Aeronaves** — cadastrar, listar e consultar frota por matrícula
- 🗺️ **Gestão de Voos** — criar voos com rota (IATA), horário, aeronave e preços por classe
- 🧑‍✈️ **Gestão de Passageiros** — cadastrar, pesquisar por nome ou passaporte
- 🎫 **Reservas e Bilhetes** — efectuar reserva com assento automático, cancelar, emitir bilhete
- 💰 **Preço Dinâmico** — calculado automaticamente com base na antecedência da compra
- ✔️ **Check-In** — validação da janela permitida (24h a 1h antes da partida)
- 🚪 **Embarque** — confirmação de embarque após check-in
- 📊 **Relatório Geral** — aeronaves, voos, reservas por estado e receita total

---

## 📁 Estrutura do Projecto

```
SistemaCompanhiaAerea/
│
├── SistemaCompanhiaAerea.sln              ← Solução Visual Studio 2015
│
└── SistemaCompanhiaAerea/
    ├── Program.cs                         ← Ponto de entrada + menu interactivo
    │
    ├── Enums/
    │   ├── ClasseVoo.cs                   ← Economica | Executiva | PrimeiraClasse
    │   └── EstadoReserva.cs               ← Activa | Cancelada | CheckInRealizado | Embarcado
    │
    ├── Exceptions/
    │   ├── AssentoOcupadoException.cs
    │   ├── VooLotadoException.cs
    │   └── CheckInForaDoPrazoException.cs
    │
    ├── Models/
    │   ├── Assento.cs                     ← Número, Classe, estado de ocupação
    │   ├── Aeronave.cs                    ← Matrícula, Tipo, lista de Assentos
    │   ├── Voo.cs                         ← Rota, horário, Aeronave, preços
    │   ├── Passageiro.cs                  ← Dados pessoais e passaporte
    │   ├── Bilhete.cs                     ← Compõe a Reserva
    │   └── Reserva.cs                     ← Passageiro ↔ Voo + Bilhete
    │
    └── Services/
        └── GestorCompanhia.cs             ← Coordenação central de todas as operações
```

---

## 🔗 Classes e Relações UML

```
GestorCompanhia
    │ agrega
    ├──▶ List<Aeronave>
    │         └── List<Assento>
    ├──▶ List<Voo>
    │         └──▶ Aeronave
    ├──▶ List<Passageiro>
    └──▶ List<Reserva>
              ├──▶ Passageiro    (associação)
              ├──▶ Voo           (associação)
              └── ◆ Bilhete      (composição)
```

| Classe | Responsabilidade |
|---|---|
| `Aeronave` | Frota: tipo, capacidade, assentos por classe |
| `Voo` | Rota, horário, aeronave, reservas e preços dinâmicos |
| `Passageiro` | Dados pessoais e número de passaporte |
| `Assento` | Número, classe e estado (livre / ocupado) |
| `Bilhete` | Emitido automaticamente com cada reserva |
| `Reserva` | Associa Passageiro ↔ Voo; contém Bilhete por composição |
| `GestorCompanhia` | Gestor central: cadastros, voos, reservas, check-in |

---

## ⚠️ Excepções Personalizadas

| Excepção | Quando é lançada |
|---|---|
| `AssentoOcupadoException` | Tentativa de reservar um assento já ocupado |
| `VooLotadoException` | Voo sem lugares disponíveis na classe pretendida |
| `CheckInForaDoPrazoException` | Check-in fora da janela permitida (24h a 1h antes da partida) |

---

## 💰 Cálculo de Preço Dinâmico

O método `Voo.CalcularPreco()` aplica um multiplicador ao preço base conforme a antecedência da compra:

| Antecedência | Multiplicador | Resultado |
|---|:---:|---|
| 60 dias ou mais | `× 0,70` | 🟢 30% de desconto |
| 30 a 59 dias | `× 0,85` | 🟢 15% de desconto |
| 14 a 29 dias | `× 1,00` | 🟡 Preço base |
| 7 a 13 dias | `× 1,15` | 🔴 15% adicional |
| Menos de 7 dias | `× 1,30` | 🔴 30% adicional |

---

## 🚀 Como Executar

### Pré-requisitos
- Visual Studio 2015 ou superior
- .NET Framework 4.6

### Passos

```bash
# 1. Clona o repositório
git clone https://github.com/GrupoDoze/Trabalho_poo_II.git

# 2. Abre o ficheiro de solução
SistemaCompanhiaAerea.sln

# 3. Compila o projecto
Build → Build Solution   (F6)

# 4. Executa
Debug → Start Without Debugging   (Ctrl + F5)
```

---

## 🗂️ Dados de Demonstração

Ao iniciar, o sistema carrega automaticamente dados de exemplo:

| Categoria | Quantidade | Exemplos |
|---|:---:|---|
| ✈️ Aeronaves | 3 | Airbus A320, Boeing 737-800, Embraer E190 |
| 🗺️ Voos | 4 | LIS→OPO, LIS→FAO, OPO→LIS, LIS→PDL |
| 🧑‍✈️ Passageiros | 5 | Maria Silva, João Santos, Ana Ferreira, Carlos Mendes, Sofia Costa |
| 🎫 Reservas | 5 | 1 Executiva · 3 Económicas · 1 Primeira Classe |

---

## 👥 Autores

<div align="center">

| Nome | Função |
|---|---|
| **Rosa Sozinho Garcia** | Desenvolvimento & Documentação |
| **Henriques Calenda Cassanda** | Desenvolvimento & Testes |
| **Luyeye Pedro** | Desenvolvimento & Modelação UML |

**Turma:** 101 &nbsp;|&nbsp; **Ano:** 3º &nbsp;|&nbsp; **Período:** Manhã  
**Orientador:** Joaquim João Nsaku Ventura

</div>

---

<div align="center">

**Instituto Superior Politécnico — UÍGE · 2026**  
Departamento de Engenharia Informática

</div>
