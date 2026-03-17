# Labor Market System Design

## Overview
The Labor Market system simulates the dynamic relationship between the workforce (Citizens) and employers (Workplace Buildings). It governs employment, wages, social security, and collective bargaining (Unions/Strikes).

## Core Components

### 1. JobPosition
A specific role within a building.
- `string title`
- `EducationLevel requiredEducation`
- `float baseSalary`
- `Citizen currentEmployee`
- `Building workplace`

### 2. LaborContract
An agreement between a Citizen and a Workplace.
- `Citizen employee`
- `Building employer`
- `float agreedSalary`
- `int monthsDuration`

### 3. Union
A collective organization representing workers.
- `string name`
- `List<Citizen> members`
- `float influence` (based on member count and industry share)
- `List<Demand> activeDemands`
- `float dissatisfaction`

### 4. JobMarketManager (Singleton)
The central engine for matching labor supply and demand.
- `List<JobPosition> vacancies`
- `void MatchLabor()`: Monthly algorithm to fill vacancies.
- `void ProcessUnions()`: Monthly check for union formation and strikes.

## Matching Algorithm
1. **Filter**: Identify all unemployed Citizens and open `JobPositions`.
2. **Sort (Supply)**: Citizens sorted by unemployment duration (desc) and education level (desc).
3. **Sort (Demand)**: Vacancies sorted by salary (desc) and prestige.
4. **Distance Heuristic**: Citizens prefer jobs closer to their `home` building.
5. **Acceptance**: Citizens evaluate offers based on `salaryExpectation` (derived from education and age).

## Social Dynamics
- **Unions**: Form when worker dissatisfaction is high or wages are significantly below national average.
- **Strikes**: Triggered if Union demands (wages, safety, hours) are ignored. A strike stops all production and consumption in the affected building.
- **Political Impact**: The "Communists" faction gains loyalty from strong unions, while "Capitalists" lose loyalty.

## Economic Integration
- **Income Tax**: Deducted automatically from monthly salaries.
- **Social Security**: Employer and employee contributions stored in a national fund for pensions/healthcare.
- **Corporate Policy**: Corporations can set salary multipliers (e.g., 1.2x for "Elite" status or 0.8x for "Efficiency" mode).

## UI/UX
- **Labor Overview**: Global unemployment rate, average salary, union influence.
- **Workplace Panel**: Manage individual job slots, fire workers, set salary levels.
- **Citizen Profile**: View current job, contract details, and job satisfaction.
