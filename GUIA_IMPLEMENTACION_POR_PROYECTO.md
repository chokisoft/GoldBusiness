```markdown
# 📘 Guía de Implementación por Proyecto
**Solución:** GoldBusiness  
**Branch:** staging  
**Repositorio:** https://github.com/chokisoft/GoldBusiness

---

## 🏗️ Estructura de la Solución

```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
│
├── 📂 GoldBusiness.Domain          # Capa de Dominio (Entidades, DTOs)
├── 📂 GoldBusiness.Infrastructure  # Capa de Infraestructura (Repositorios, BD)
├── 📂 GoldBusiness.Application     # Capa de Aplicación (Servicios, Lógica)
├── 📂 GoldBusiness.WebApi          # Capa de API (Controllers, Endpoints)
└── 📂 GoldBusiness.Client          # Frontend Angular
```

---

## 🎯 Orden de Implementación por Proyecto

### **Paso 1: GoldBusiness.Infrastructure (Repositories)**

#### **1.1 Interface del Repositorio**

**📂 Proyecto:** `GoldBusiness.Infrastructure`  
**📁 Carpeta:** `Repositories\`  
**📄 Archivo:** `I[Entity]Repository.cs`  
**🔧 Acción:** Agregar método `GetPagedAsync()` + `AddAsync()` + `UpdateAsync()`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Infrastructure\
      └── Repositories\
          └── I[Entity]Repository.cs
```

**Código a agregar:**
```
using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface I[Entity]Repository
    {
        Task<IEnumerable<[Entity]>> GetAllAsync();
        
        // ✅ AGREGAR ESTE MÉTODO
        Task<(IEnumerable<[Entity]> Items, int Total)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? termino = null);
        
        Task<[Entity]?> GetByIdAsync(int id);
        Task<[Entity]?> GetByCodigoAsync(string codigo, bool includeCanceled = false);
        
        // ✅ VERIFICAR QUE EXISTAN ESTOS
        Task<[Entity]> AddAsync([Entity] entity);
        Task UpdateAsync([Entity] entity);
    }
}
```

**✅ Checklist:**
- [ ] Interfaz creada o actualizada
- [ ] Método `GetPagedAsync()` agregado
- [ ] Métodos `AddAsync()` y `UpdateAsync()` existen
- [ ] Compilación exitosa

---

#### **1.2 Implementación del Repositorio**

**📂 Proyecto:** `GoldBusiness.Infrastructure`  
**📁 Carpeta:** `Repositories\`  
**📄 Archivo:** `[Entity]Repository.cs`  
**🔧 Acción:** Implementar `GetPagedAsync()` + verificar `AddAsync()`/`UpdateAsync()`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Infrastructure\
      └── Repositories\
          └── [Entity]Repository.cs
```

**Código a agregar:**

##### **Para entidades SIMPLES (sin relaciones):**
```
// Ejemplo: GrupoCuenta, Linea, UnidadMedida

public async Task<(IEnumerable<[Entity]> Items, int Total)> GetPagedAsync(
    int page, 
    int pageSize, 
    string? termino = null)
{
    var query = _context.[Entity]
        .AsNoTracking()
        .Where(e => !e.Cancelado);

    // Búsqueda en Código y Descripción
    if (!string.IsNullOrWhiteSpace(termino))
    {
        var lowerTerm = termino.ToLower();
        query = query.Where(e => 
            e.Codigo.ToLower().Contains(lowerTerm) ||
            e.Descripcion.ToLower().Contains(lowerTerm)
        );
    }

    var total = await query.CountAsync();

    var items = await query
        .Include(e => e.Translations)
        .OrderBy(e => e.Codigo)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, total);
}

// Verificar que existan estos métodos
public async Task<[Entity]> AddAsync([Entity] entity)
{
    _context.[Entity].Add(entity);
    await _context.SaveChangesAsync();
    return entity;
}

public async Task UpdateAsync([Entity] entity)
{
    _context.[Entity].Update(entity);
    await _context.SaveChangesAsync();
}
```

##### **Para entidades CON relaciones (1 nivel):**
```
// Ejemplo: SubGrupoCuenta → GrupoCuenta, SubLinea → Linea

public async Task<(IEnumerable<[Entity]> Items, int Total)> GetPagedAsync(
    int page, 
    int pageSize, 
    string? termino = null,
    int? parentEntityId = null)  // ✅ Filtro adicional opcional
{
    var query = _context.[Entity]
        .AsNoTracking()
        .Where(e => !e.Cancelado);

    // Búsqueda en múltiples campos
    if (!string.IsNullOrWhiteSpace(termino))
    {
        var lowerTerm = termino.ToLower();
        query = query.Where(e => 
            e.Codigo.ToLower().Contains(lowerTerm) ||
            e.Descripcion.ToLower().Contains(lowerTerm) ||
            e.ParentEntity!.Descripcion.ToLower().Contains(lowerTerm)
        );
    }

    // Filtro adicional (opcional)
    if (parentEntityId.HasValue)
        query = query.Where(e => e.ParentEntityId == parentEntityId.Value);

    var total = await query.CountAsync();

    var items = await query
        .Include(e => e.Translations)
        .Include(e => e.ParentEntity)
            .ThenInclude(p => p!.Translations)
        .OrderBy(e => e.Codigo)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, total);
}
```

##### **Para entidades CON relaciones en cascada:**
```
// Ejemplo: Cuenta → SubGrupoCuenta → GrupoCuenta

public async Task<(IEnumerable<Cuenta> Items, int Total)> GetPagedAsync(
    int page, 
    int pageSize, 
    string? termino = null,
    int? subGrupoCuentaId = null)
{
    var query = _context.Cuenta
        .AsNoTracking()
        .Where(c => !c.Cancelado);

    if (!string.IsNullOrWhiteSpace(termino))
    {
        var lowerTerm = termino.ToLower();
        query = query.Where(c => 
            c.Codigo.ToLower().Contains(lowerTerm) ||
            c.Descripcion.ToLower().Contains(lowerTerm) ||
            c.SubGrupoCuenta!.Descripcion.ToLower().Contains(lowerTerm) ||
            c.SubGrupoCuenta!.GrupoCuenta!.Descripcion.ToLower().Contains(lowerTerm)
        );
    }

    if (subGrupoCuentaId.HasValue)
        query = query.Where(c => c.SubGrupoCuentaId == subGrupoCuentaId.Value);

    var total = await query.CountAsync();

    var items = await query
        .Include(c => c.Translations)
        .Include(c => c.SubGrupoCuenta)
            .ThenInclude(sg => sg!.Translations)
        .Include(c => c.SubGrupoCuenta!.GrupoCuenta)
            .ThenInclude(g => g!.Translations)
        .OrderBy(c => c.Codigo)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, total);
}
```

**✅ Checklist:**
- [ ] Método `GetPagedAsync()` implementado
- [ ] Usa `AsNoTracking()` para consultas de solo lectura
- [ ] Búsqueda case-insensitive con `.ToLower()`
- [ ] Incluye `.Include(e => e.Translations)` para i18n
- [ ] Incluye relaciones necesarias con `.ThenInclude()`
- [ ] Métodos `AddAsync()` y `UpdateAsync()` implementados
- [ ] Compilación exitosa

---

### **Paso 2: GoldBusiness.Application (Services)**

#### **2.1 Interface del Servicio**

**📂 Proyecto:** `GoldBusiness.Application`  
**📁 Carpeta:** `Interfaces\`  
**📄 Archivo:** `I[Entity]Service.cs`  
**🔧 Acción:** Agregar método `GetPagedAsync()`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Application\
      └── Interfaces\
          └── I[Entity]Service.cs
```

**Código a agregar:**

##### **Para entidades SIMPLES:**
```
using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface I[Entity]Service
    {
        Task<IEnumerable<[Entity]DTO>> GetAllAsync(string lang = "es");
        
        // ✅ AGREGAR ESTE MÉTODO (sin filtro adicional)
        Task<(IEnumerable<[Entity]DTO> Items, int Total)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? termino = null, 
            string lang = "es");
        
        Task<[Entity]DTO?> GetByIdAsync(int id, string lang = "es");
        Task<[Entity]DTO> CreateAsync([Entity]DTO dto, string user, string lang = "es");
        Task<[Entity]DTO> UpdateAsync(int id, [Entity]DTO dto, string user, string lang = "es");
        Task<[Entity]DTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
```

##### **Para entidades CON relaciones (con filtro adicional):**
```
// Ejemplo: SubGrupoCuenta, Cuenta, Municipio, Provincia

public interface I[Entity]Service
{
    Task<IEnumerable<[Entity]DTO>> GetAllAsync(string lang = "es");
    
    // ✅ CON filtro adicional (parentEntityId)
    Task<(IEnumerable<[Entity]DTO> Items, int Total)> GetPagedAsync(
        int page, 
        int pageSize, 
        string? termino = null,
        int? parentEntityId = null,  // ✅ Filtro opcional
        string lang = "es");
    
    // Resto de métodos...
}
```

**✅ Checklist:**
- [ ] Interface actualizada
- [ ] Método `GetPagedAsync()` agregado con parámetros correctos
- [ ] Compilación exitosa

---

#### **2.2 Implementación del Servicio**

**📂 Proyecto:** `GoldBusiness.Application`  
**📁 Carpeta:** `Services\`  
**📄 Archivo:** `[Entity]Service.cs`  
**🔧 Acción:** Implementar `GetPagedAsync()`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Application\
      └── Services\
          └── [Entity]Service.cs
```

**Código a agregar:**

##### **Para entidades SIMPLES:**
```
public async Task<(IEnumerable<[Entity]DTO> Items, int Total)> GetPagedAsync(
    int page, 
    int pageSize, 
    string? termino = null, 
    string lang = "es")
{
    var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino);
    var dtos = items.Select(e => MapToDTO(e, lang)).ToList();
    return (dtos, total);
}
```

##### **Para entidades CON filtro adicional:**
```
public async Task<(IEnumerable<[Entity]DTO> Items, int Total)> GetPagedAsync(
    int page, 
    int pageSize, 
    string? termino = null,
    int? parentEntityId = null,
    string lang = "es")
{
    var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino, parentEntityId);
    var dtos = items.Select(e => MapToDTO(e, lang)).ToList();
    return (dtos, total);
}
```

**✅ Checklist:**
- [ ] Método implementado
- [ ] Llama al repositorio correctamente
- [ ] Usa `MapToDTO()` para convertir a DTOs
- [ ] Respeta el parámetro de idioma
- [ ] Compilación exitosa

---

### **Paso 3: GoldBusiness.WebApi (Controllers)**

**📂 Proyecto:** `GoldBusiness.WebApi`  
**📁 Carpeta:** `Controllers\`  
**📄 Archivo:** `[Entity]Controller.cs`  
**🔧 Acción:** Agregar endpoint `[HttpGet("paged")]`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.WebApi\
      └── Controllers\
          └── [Entity]Controller.cs
```

**Código a agregar:**

##### **Importar namespaces (IMPORTANTE):**
```
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs; // ✅ AGREGAR ESTE using
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
```

##### **Para entidades SIMPLES:**
```
[HttpGet]
[ProducesResponseType(StatusCodes.Status200OK)]
public async Task<ActionResult<IEnumerable<[Entity]DTO>>> Get()
{
    var lang = GetCurrentLanguage();
    var result = await _service.GetAllAsync(lang);
    return Ok(result);
}

// ✅ AGREGAR ESTE ENDPOINT
[HttpGet("paged")]
[ProducesResponseType(StatusCodes.Status200OK)]
public async Task<ActionResult> GetPaged(
    [FromQuery] int page = 1, 
    [FromQuery] int pageSize = 50, 
    [FromQuery] string? term = null)
{
    var lang = GetCurrentLanguage();
    var (items, total) = await _service.GetPagedAsync(page, pageSize, term, lang);
    return Ok(new { items, total });
}

[HttpGet("{id}")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<[Entity]DTO>> Get(int id)
{
    var lang = GetCurrentLanguage();
    var dto = await _service.GetByIdAsync(id, lang);
    return dto == null ? NotFound() : Ok(dto);
}
```

##### **Para entidades CON filtro adicional:**
```
// Ejemplo: SubGrupoCuenta, Cuenta, Municipio, Provincia

[HttpGet("paged")]
[ProducesResponseType(StatusCodes.Status200OK)]
public async Task<ActionResult> GetPaged(
    [FromQuery] int page = 1, 
    [FromQuery] int pageSize = 50, 
    [FromQuery] string? term = null,
    [FromQuery] int? parentEntityId = null)  // ✅ Filtro adicional
{
    var lang = GetCurrentLanguage();
    var (items, total) = await _service.GetPagedAsync(page, pageSize, term, parentEntityId, lang);
    return Ok(new { items, total });
}
```

**✅ Checklist:**
- [ ] `using GoldBusiness.Domain.DTOs;` agregado
- [ ] Endpoint `[HttpGet("paged")]` agregado
- [ ] Retorna `new { items, total }`
- [ ] Usa `GetCurrentLanguage()` para i18n
- [ ] Compilación exitosa
- [ ] API compila sin errores

---

### **Paso 4: GoldBusiness.Client (Frontend Angular)**

#### **4.1 Servicio TypeScript**

**📂 Proyecto:** `GoldBusiness.Client`  
**📁 Carpeta:** `src\app\services\`  
**📄 Archivo:** `[entity].service.ts`  
**🔧 Acción:** Agregar método `getPaged()` y interface `PagedResult<T>`

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Client\
      └── src\
          └── app\
              └── services\
                  └── [entity].service.ts
```

**Código a agregar/modificar:**

```
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface [Entity]DTO {
  id?: number;
  codigo: string;
  descripcion: string;
  cancelado?: boolean;
  creadoPor?: string;
  fechaHoraCreado?: string;
  modificadoPor?: string;
  fechaHoraModificado?: string;
  // Agregar propiedades específicas de la entidad
}

// ✅ AGREGAR ESTA INTERFACE
export interface PagedResult<T> {
  items: T[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class [Entity]Service {
  constructor(private api: ApiService) {}

  // ✅ AGREGAR ESTE MÉTODO
  getPaged(page: number = 1, pageSize: number = 50, term?: string): Observable<PagedResult<[Entity]DTO>> {
    let url = `[Entity]/paged?page=${page}&pageSize=${pageSize}`;
    if (term) url += `&term=${encodeURIComponent(term)}`;
    return this.api.get<PagedResult<[Entity]DTO>>(url);
  }

  getAll(): Observable<[Entity]DTO[]> {
    console.warn('⚠️ [Entity]Service.getAll() puede ser lento. Considera usar getPaged()');
    return this.api.get<[Entity]DTO[]>('[Entity]');
  }

  getById(id: number): Observable<[Entity]DTO> {
    return this.api.get<[Entity]DTO>(`[Entity]/${id}`);
  }

  create(data: [Entity]DTO): Observable<[Entity]DTO> {
    return this.api.post<[Entity]DTO>('[Entity]', data);
  }

  update(id: number, data: [Entity]DTO): Observable<[Entity]DTO> {
    return this.api.put<[Entity]DTO>(`[Entity]/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`[Entity]/${id}`);
  }
}
```

**Para entidades con filtro adicional:**
```
// Ejemplo: SubGrupoCuenta, Cuenta, Municipio

getPaged(page: number = 1, pageSize: number = 50, term?: string, parentEntityId?: number): Observable<PagedResult<[Entity]DTO>> {
  let url = `[Entity]/paged?page=${page}&pageSize=${pageSize}`;
  if (term) url += `&term=${encodeURIComponent(term)}`;
  if (parentEntityId) url += `&parentEntityId=${parentEntityId}`;
  return this.api.get<PagedResult<[Entity]DTO>>(url);
}
```

**✅ Checklist:**
- [ ] Interface `PagedResult<T>` agregada
- [ ] Método `getPaged()` agregado
- [ ] Usa `encodeURIComponent()` para el término
- [ ] DTO actualizado con propiedades correctas
- [ ] Compilación exitosa (`npm start` sin errores)

---

#### **4.2 Componente TypeScript de Lista**

**📂 Proyecto:** `GoldBusiness.Client`  
**📁 Carpeta:** `src\app\pages\[entity]\[entity]-list\`  
**📄 Archivo:** `[entity]-list.component.ts`  
**🔧 Acción:** Migrar de paginación cliente a servidor

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Client\
      └── src\
          └── app\
              └── pages\
                  └── [entity]\
                      └── [entity]-list\
                          └── [entity]-list.component.ts
```

**Código completo:**

```
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { [Entity]Service, [Entity]DTO } from '../../../services/[entity].service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-[entity]-list',
  templateUrl: './[entity]-list.component.html',
  styleUrls: ['./[entity]-list.component.css']
})
export class [Entity]ListComponent implements OnInit, OnDestroy {
  // ✅ SOLO ESTAS PROPIEDADES (eliminar filteredX, paginatedX si existen)
  entities: [Entity]DTO[] = [];
  loading = false;
  searching = false;
  error: string | null = null;

  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  Math = Math;

  private languageSubscription?: Subscription;
  private searchDebounceTimer?: any;

  constructor(
    private entityService: [Entity]Service,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData(true);
    
    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 [Entity]List: Idioma cambiado, recargando...');
        this.loadData(true);
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
  }

  loadData(isInitialLoad: boolean = false): void {
    if (isInitialLoad) this.loading = true;
    else this.searching = true;
    
    this.error = null;
    const searchTerm = this.searchTerm.trim() || undefined;
    
    this.entityService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /[Entity]/paged response:', response);
          this.entities = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error:', err);
          this.error = `Error al cargar: ${err?.message ?? err.statusText}`;
        }
      });
  }

  onSearch(): void {
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
    this.searchDebounceTimer = setTimeout(() => {
      this.currentPage = 1;
      this.loadData(false);
    }, 500);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadData(false);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadData(false);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadData(false);
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
    
    if (endPage - startPage < maxVisiblePages - 1) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  onPageSizeChange(newSize?: number | string): void {
    const parsed = Number(newSize);
    if (!isNaN(parsed) && parsed > 0) this.pageSize = parsed;
    else if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
    
    this.currentPage = 1;
    this.loadData(false);
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Estás seguro de eliminar "${descripcion}"?`)) {
      this.entityService.delete(id).subscribe({
        next: () => {
          if (this.entities.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData(false);
        },
        error: (err) => {
          this.error = 'Error al eliminar';
          console.error('Error:', err);
        }
      });
    }
  }
}
```

**✅ Checklist:**
- [ ] Eliminadas propiedades `filtered[Entity][]`, `paginated[Entity][]`
- [ ] Agregada propiedad `searching: boolean`
- [ ] Agregada propiedad `totalItems: number`
- [ ] `loadData()` tiene parámetro `isInitialLoad`
- [ ] Debounce de 500ms en `onSearch()`
- [ ] `ngOnDestroy()` limpia el timer
- [ ] Compilación exitosa

---

#### **4.3 Template HTML de Lista**

**📂 Proyecto:** `GoldBusiness.Client`  
**📁 Carpeta:** `src\app\pages\[entity]\[entity]-list\`  
**📄 Archivo:** `[entity]-list.component.html`  
**🔧 Acción:** Actualizar template para paginación del servidor

**Ruta completa:**
```
F:\Documents\Visual Studio 18\Projects\GoldBusiness\
  └── GoldBusiness.Client\
      └── src\
          └── app\
              └── pages\
                  └── [entity]\
                      └── [entity]-list\
                          └── [entity]-list.component.html
```

**Código completo:**

```
<div class="page-container">
  <div class="page-header">
    <div class="header-content">
      <h1 class="page-title">🎯 {{ '[entity].title' | translate }}</h1>
      <p class="page-subtitle">{{ '[entity].subtitle' | translate }}</p>
    </div>
    <div class="header-actions">
      <button class="btn btn-primary" [routerLink]="['nuevo']">
        <span class="btn-icon">➕</span>
        {{ 'common.new' | translate }}
      </button>
    </div>
  </div>

  <div class="alert alert-error" *ngIf="error">
    <span class="alert-icon">⚠️</span>
    {{ error }}
  </div>

  <!-- ✅ IMPORTANTE: Condición *ngIf="!loading" (no ocultar durante búsqueda) -->
  <div class="card" *ngIf="!loading">
    <div class="card-header">
      <div class="search-container">
        <input type="text"
               class="form-control search-input"
               [(ngModel)]="searchTerm"
               (input)="onSearch()"
               [placeholder]="'common.search' | translate"
               [class.searching]="searching" />
        <!-- ✅ Indicador visual durante búsqueda -->
        <span class="search-icon">
          <ng-container *ngIf="!searching">🔍</ng-container>
          <ng-container *ngIf="searching">⏳</ng-container>
        </span>
      </div>
      <div class="page-size-selector">
        <label>{{ 'common.show' | translate }}:</label>
        <select [(ngModel)]="pageSize" 
                (ngModelChange)="onPageSizeChange($event)" 
                class="form-control">
          <option [value]="5">5</option>
          <option [value]="10">10</option>
          <option [value]="25">25</option>
          <option [value]="50">50</option>
          <option [value]="100">100</option>
        </select>
      </div>
    </div>

    <div class="card-body">
      <div class="table-responsive">
        <table class="data-table">
          <thead>
            <tr>
              <th>{{ '[entity].codigo' | translate }}</th>
              <th>{{ '[entity].descripcion' | translate }}</th>
              <th class="actions-column">{{ 'common.actions' | translate }}</th>
            </tr>
          </thead>
          <tbody>
            <!-- ✅ IMPORTANTE: Usar 'entities' directamente (no filteredX ni paginatedX) -->
            <tr *ngFor="let item of entities">
              <td>{{ item.codigo }}</td>
              <td>{{ item.descripcion }}</td>
              <td class="actions-column">
                <div class="action-buttons">
                  <button class="btn btn-sm btn-info"
                          [routerLink]="[item.id]"
                          [title]="'common.view' | translate">
                    <span>👁️</span>
                  </button>
                  <button class="btn btn-sm btn-warning"
                          [routerLink]="['editar', item.id]"
                          [title]="'common.edit' | translate">
                    <span>✏️</span>
                  </button>
                  <button class="btn btn-sm btn-danger"
                          (click)="delete(item.id!, item.descripcion)"
                          [title]="'common.delete' | translate">
                    <span>🗑️</span>
                  </button>
                </div>
              </td>
            </tr>
            <tr *ngIf="entities.length === 0">
              <td colspan="3" class="text-center">
                {{ searchTerm ? ('common.noResults' | translate) : ('common.noData' | translate) }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- ✅ Paginación Estandarizada -->
    <div class="card-footer" *ngIf="totalPages > 0">
      <div class="pagination-info">
        {{ 'common.showing' | translate }} 
        {{ (currentPage - 1) * pageSize + 1 }} - 
        {{ Math.min(currentPage * pageSize, totalItems) }} 
        {{ 'common.of' | translate }} 
        {{ totalItems }}
      </div>
      <div class="pagination">
        <button 
          class="btn btn-sm btn-secondary" 
          (click)="previousPage()"
          [disabled]="currentPage === 1">
          ‹ {{ 'common.previous' | translate }}
        </button>
        
        <button 
          *ngFor="let page of getPageNumbers()"
          class="btn btn-sm"
          [class.btn-primary]="page === currentPage"
          [class.btn-secondary]="page !== currentPage"
          (click)="goToPage(page)">
          {{ page }}
        </button>

        <button 
          class="btn btn-sm btn-secondary" 
          (click)="nextPage()"
          [disabled]="currentPage === totalPages">
          {{ 'common.next' | translate }} ›
        </button>
      </div>
    </div>
  </div>

  <!-- ✅ Loader solo para carga inicial -->
  <div *ngIf="loading" class="loading-container">
    <app-loader [translateKey]="'common.loading'" 
                [center]="true" 
                [size]="28" 
                [inline]="false">
    </app-loader>
  </div>
</div>
```

**✅ Checklist:**
- [ ] Condición `*ngIf="!loading"` en el card
- [ ] Icono cambia de 🔍 a ⏳ cuando `searching`
- [ ] Usa `entities` directamente en `*ngFor`
- [ ] Usa `totalItems` (del servidor) en info de paginación
- [ ] Botones con texto `‹ Anterior` y `Siguiente ›`
- [ ] Loader solo cuando `loading` (no cuando `searching`)
- [ ] Compilación exitosa (`npm start`)

---

## 📊 Tabla de Seguimiento por Entidad

| Entidad | Proyecto | Archivos a Modificar |
|---------|----------|----------------------|
| **GrupoCuenta** | Infrastructure | `IGrupoCuentaRepository.cs` + `GrupoCuentaRepository.cs` |
|  | Application | `IGrupoCuentaService.cs` + `GrupoCuentaService.cs` |
|  | WebApi | `GrupoCuentaController.cs` |
|  | Client | `grupo-cuenta.service.ts` + `grupo-cuenta-list.component.ts` + `.html` |
| **SubGrupoCuenta** | Infrastructure | `ISubGrupoCuentaRepository.cs` + `SubGrupoCuentaRepository.cs` |
|  | Application | `ISubGrupoCuentaService.cs` + `SubGrupoCuentaService.cs` |
|  | WebApi | `SubGrupoCuentaController.cs` |
|  | Client | `sub-grupo-cuenta.service.ts` + `subgrupo-cuenta-list.component.ts` + `.html` |
| **Cuenta** | Infrastructure | `ICuentaRepository.cs` + `CuentaRepository.cs` |
|  | Application | `ICuentaService.cs` + `CuentaService.cs` |
|  | WebApi | `CuentaController.cs` |
|  | Client | `cuenta.service.ts` + `cuenta-list.component.ts` + `.html` |
| **Linea** | Infrastructure | `ILineaRepository.cs` + `LineaRepository.cs` |
|  | Application | `ILineaService.cs` + `LineaService.cs` |
|  | WebApi | `LineaController.cs` |
|  | Client | `linea.service.ts` + `linea-list.component.ts` + `.html` |
| **SubLinea** | Infrastructure | `ISubLineaRepository.cs` + `SubLineaRepository.cs` |
|  | Application | `ISubLineaService.cs` + `SubLineaService.cs` |
|  | WebApi | `SubLineaController.cs` |
|  | Client | `sub-linea.service.ts` + `sub-linea-list.component.ts` + `.html` |
| **UnidadMedida** | Infrastructure | `IUnidadMedidaRepository.cs` + `UnidadMedidaRepository.cs` |
|  | Application | `IUnidadMedidaService.cs` + `UnidadMedidaService.cs` |
|  | WebApi | `UnidadMedidaController.cs` |
|  | Client | `unidad-medida.service.ts` + `unidad-medida-list.component.ts` + `.html` |

---

## 🎯 Checklist General de Validación

Después de implementar cada entidad, verifica:

### **Backend (.NET)**
```
# En la raíz de la solución
cd "F:\Documents\Visual Studio 18\Projects\GoldBusiness"

# Limpiar y compilar
dotnet clean
dotnet build

# Ejecutar
dotnet run --project GoldBusiness.WebApi
```

**✅ Verificar:**
- [ ] Compilación sin errores
- [ ] API inicia correctamente
- [ ] Endpoint `/api/[Entity]/paged` responde
- [ ] Prueba manual: `https://localhost:7000/api/[Entity]/paged?page=1&pageSize=10`

### **Frontend (Angular)**
```
# En la carpeta del cliente
cd "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Client"

# Limpiar caché (si hay errores)
rd /s /q node_modules
rd /s /q .angular
npm install

# Iniciar
npm start
```

**✅ Verificar:**
- [ ] Compilación sin errores
- [ ] Aplicación inicia en `http://localhost:4200`
- [ ] Lista carga correctamente
- [ ] Búsqueda funciona sin "refresh"
- [ ] Paginación funciona
- [ ] Cambio de idioma funciona

### **Git (Control de Versiones)**
```
# Después de cada entidad completada
git add .
git commit -m "feat: Add server pagination to [Entity]"
git push origin staging
```

---

## 📝 Notas Finales

1. **Orden recomendado de implementación:**
   - GrupoCuenta (simple, sin relaciones)
   - SubGrupoCuenta (1 relación)
   - Cuenta (2 relaciones en cascada)
   - Linea (simple)
   - SubLinea (1 relación)
   - UnidadMedida (simple)

2. **Backup antes de comenzar:**
```

   git checkout -b backup-antes-paginacion
   git push origin backup-antes-paginacion
   git checkout staging
```

3. **Si algo sale mal:**
```
   git checkout staging
   git reset --hard HEAD
   git clean -fd
```

---

**¡Listo para imprimir y usar como guía! 📄✨**
```

Este es el archivo Markdown completo sin omisiones. Puedes copiarlo, pegarlo en un archivo `.md` y usarlo como referencia completa para implementar la paginación del servidor en todas tus entidades. 🚀📘