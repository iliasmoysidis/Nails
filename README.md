# Domain Aggregates Overview

This document lists all aggregates, their roots, and their internal entities.

---

## **Aggregate: User**

**Root:** `User`

---

## **Aggregate: StoreStaff**

**Root:** `StoreStaffManager`  
**Entities:**

-   `StoreOwner`
-   `StoreStaff`

---

## **Aggregate: StoreSchedule**

**Root:** `StoreScheduleManager`  
**Entities:**

-   `StoreSchedule`
-   `StoreScheduleSpecial`

---

## **Aggregate: StaffSchedule**

**Root:** `StoreStaffScheduleManager`  
**Entities:**

-   `StoreStaffSchedule`
-   `StoreStaffScheduleSpecial`

---

## **Aggregate: StoreServiceCatalog**

**Root:** `StoreServiceManager`  
**Entities:**

-   `Service`
-   `StaffService`

---

## **Aggregate: Appointment**

**Root:** `Appointment`

---
