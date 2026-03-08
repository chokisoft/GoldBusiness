using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoldBusiness.CodeGen
{
    public class CodeGenService
    {
        private readonly string _clientRoot;
        private readonly EntityMetadata _metadata;
        private readonly bool _previewMode;

        public CodeGenService(string clientRoot, EntityMetadata metadata, bool previewMode = false)
        {
            _clientRoot = clientRoot;
            _metadata = metadata;
            _previewMode = previewMode;
        }

        /// <summary>
        /// Genera todos los archivos Angular para una entidad
        /// </summary>
        public void GenerateAngularFiles()
        {
            Console.WriteLine($"🚀 Generando archivos para entidad: {_metadata.EntityName}");

            // 1. Generar Service
            GenerateService();

            // 2. Generar componente List
            GenerateListComponent();

            // 3. Generar componente Form
            GenerateFormComponent();

            // 4. Generar componente Detail
            GenerateDetailComponent();

            if (_previewMode)
            {
                Console.WriteLine($"🔍 Simulación completa para {_metadata.EntityName}");
            }
            else
            {
                Console.WriteLine($"✅ Generación completa para {_metadata.EntityName}");
            }
        }

        #region Service Generation

        private void GenerateService()
        {
            var servicePath = Path.Combine(_clientRoot, "services", $"{_metadata.KebabCase}.service.ts");
            var content = GenerateServiceContent();
            WriteFile(servicePath, content);

            if (_previewMode)
            {
                Console.WriteLine($"  🔍 [PREVIEW] Service: {servicePath}");
            }
            else
            {
                Console.WriteLine($"  ✓ Service: {servicePath}");
            }
        }

        private string GenerateServiceContent()
        {
            var sb = new StringBuilder();

            // Import statements
            sb.AppendLine("import { Injectable } from '@angular/core';");
            sb.AppendLine("import { HttpClient } from '@angular/common/http';");
            sb.AppendLine("import { Observable } from 'rxjs';");
            sb.AppendLine("import { ApiService } from './api.service';");
            sb.AppendLine();

            // Interface DTO
            sb.AppendLine($"export interface {_metadata.EntityName}DTO {{");
            sb.AppendLine("  id?: number;");

            foreach (var prop in _metadata.Properties)
            {
                var tsType = MapToTypeScriptType(prop.Type);
                var optional = prop.IsNullable ? "?" : "";
                sb.AppendLine($"  {ToCamelCase(prop.Name)}{optional}: {tsType};");
            }

            // Propiedades de auditoría
            sb.AppendLine();
            sb.AppendLine("  cancelado?: boolean;");
            sb.AppendLine("  creadoPor?: string;");
            sb.AppendLine("  fechaHoraCreado?: Date;");
            sb.AppendLine("  modificadoPor?: string;");
            sb.AppendLine("  fechaHoraModificado?: Date;");
            sb.AppendLine("}");
            sb.AppendLine();

            // Service class
            sb.AppendLine("@Injectable({");
            sb.AppendLine("  providedIn: 'root'");
            sb.AppendLine("})");
            sb.AppendLine($"export class {_metadata.EntityName}Service {{");
            sb.AppendLine($"  private endpoint = '{_metadata.CamelCase}';");
            sb.AppendLine();
            sb.AppendLine("  constructor(private apiService: ApiService) {}");
            sb.AppendLine();

            // CRUD methods
            sb.AppendLine($"  getAll(): Observable<{_metadata.EntityName}DTO[]> {{");
            sb.AppendLine($"    return this.apiService.get<{_metadata.EntityName}DTO[]>(this.endpoint);");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine($"  getById(id: number): Observable<{_metadata.EntityName}DTO> {{");
            sb.AppendLine($"    return this.apiService.get<{_metadata.EntityName}DTO>(`${{this.endpoint}}/${{id}}`);");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine($"  create(dto: {_metadata.EntityName}DTO): Observable<{_metadata.EntityName}DTO> {{");
            sb.AppendLine($"    return this.apiService.post<{_metadata.EntityName}DTO>(this.endpoint, dto);");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine($"  update(id: number, dto: {_metadata.EntityName}DTO): Observable<void> {{");
            sb.AppendLine($"    return this.apiService.put<void>(`${{this.endpoint}}/${{id}}`, dto);");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  delete(id: number): Observable<void> {");
            sb.AppendLine("    return this.apiService.delete<void>(`${this.endpoint}/${id}`);");
            sb.AppendLine("  }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        #endregion

        #region List Component Generation

        private void GenerateListComponent()
        {
            var componentDir = Path.Combine(_clientRoot, "pages", _metadata.KebabCase, $"{_metadata.KebabCase}-list");

            if (!_previewMode)
            {
                Directory.CreateDirectory(componentDir);
            }

            // TypeScript
            var tsContent = GenerateListComponentTs();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-list.component.ts"), tsContent);

            // HTML
            var htmlContent = GenerateListComponentHtml();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-list.component.html"), htmlContent);

            // CSS
            var cssContent = GenerateListComponentCss();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-list.component.css"), cssContent);

            if (_previewMode)
            {
                Console.WriteLine($"  🔍 [PREVIEW] List Component: {componentDir}");
            }
            else
            {
                Console.WriteLine($"  ✓ List Component: {componentDir}");
            }
        }

        private string GenerateListComponentTs()
        {
            var sb = new StringBuilder();

            sb.AppendLine("import { Component, OnInit, OnDestroy } from '@angular/core';");
            sb.AppendLine("import { Subscription } from 'rxjs';");
            sb.AppendLine("import { skip, finalize } from 'rxjs/operators';");
            sb.AppendLine($"import {{ {_metadata.EntityName}Service, {_metadata.EntityName}DTO }} from '../../../services/{_metadata.KebabCase}.service';");
            sb.AppendLine("import { LanguageService } from '../../../services/language.service';");
            sb.AppendLine();

            sb.AppendLine("@Component({");
            sb.AppendLine($"  selector: 'app-{_metadata.KebabCase}-list',");
            sb.AppendLine($"  templateUrl: './{_metadata.KebabCase}-list.component.html',");
            sb.AppendLine($"  styleUrls: ['./{_metadata.KebabCase}-list.component.css']");
            sb.AppendLine("})");
            sb.AppendLine($"export class {_metadata.EntityName}ListComponent implements OnInit, OnDestroy {{");

            // Properties
            sb.AppendLine($"  {_metadata.PluralCamelCase}: {_metadata.EntityName}DTO[] = [];");
            sb.AppendLine($"  filtered{_metadata.PluralEntityName}: {_metadata.EntityName}DTO[] = [];");
            sb.AppendLine($"  paginated{_metadata.PluralEntityName}: {_metadata.EntityName}DTO[] = [];");
            sb.AppendLine("  loading = false;");
            sb.AppendLine("  error: string | null = null;");
            sb.AppendLine();
            sb.AppendLine("  searchTerm: string = '';");
            sb.AppendLine("  currentPage: number = 1;");
            sb.AppendLine("  pageSize: number = 10;");
            sb.AppendLine("  totalPages: number = 0;");
            sb.AppendLine("  startItem: number = 0;");
            sb.AppendLine("  endItem: number = 0;");
            sb.AppendLine("  Math = Math;");
            sb.AppendLine();
            sb.AppendLine("  private languageSubscription?: Subscription;");
            sb.AppendLine();

            // Constructor
            sb.AppendLine("  constructor(");
            sb.AppendLine($"    private {_metadata.CamelCase}Service: {_metadata.EntityName}Service,");
            sb.AppendLine("    private languageService: LanguageService");
            sb.AppendLine("  ) {}");
            sb.AppendLine();

            // ngOnInit
            sb.AppendLine("  ngOnInit(): void {");
            sb.AppendLine("    this.loadData();");
            sb.AppendLine();
            sb.AppendLine("    this.languageSubscription = this.languageService.currentLanguage$");
            sb.AppendLine("      .pipe(skip(1))");
            sb.AppendLine("      .subscribe(() => {");
            sb.AppendLine($"        console.log('🔄 {_metadata.EntityName}List: Idioma cambiado, recargando datos...');");
            sb.AppendLine("        this.loadData();");
            sb.AppendLine("      });");
            sb.AppendLine("  }");
            sb.AppendLine();

            // ngOnDestroy
            sb.AppendLine("  ngOnDestroy(): void {");
            sb.AppendLine("    this.languageSubscription?.unsubscribe();");
            sb.AppendLine("  }");
            sb.AppendLine();

            // loadData method
            AppendLoadDataMethod(sb);

            // Search and pagination methods
            AppendSearchAndPaginationMethods(sb);

            // Delete method
            sb.AppendLine();
            sb.AppendLine($"  delete(id: number, {GetPrimaryDisplayField()}: string): void {{");
            sb.AppendLine($"    if (confirm(`¿Está seguro de eliminar: \"${{{GetPrimaryDisplayField()}}}\"?`)) {{");
            sb.AppendLine($"      this.{_metadata.CamelCase}Service.delete(id).subscribe({{");
            sb.AppendLine("        next: () => this.loadData(),");
            sb.AppendLine("        error: (err) => {");
            sb.AppendLine($"          this.error = 'Error al eliminar el {_metadata.CamelCase}';");
            sb.AppendLine("          console.error('Error:', err);");
            sb.AppendLine("        }");
            sb.AppendLine("      });");
            sb.AppendLine("    }");
            sb.AppendLine("  }");

            sb.AppendLine("}");

            return sb.ToString();
        }

        private void AppendLoadDataMethod(StringBuilder sb)
        {
            sb.AppendLine("  loadData(): void {");
            sb.AppendLine("    this.loading = true;");
            sb.AppendLine("    this.error = null;");
            sb.AppendLine();
            sb.AppendLine("    Promise.resolve().then(() => {");
            sb.AppendLine($"      this.{_metadata.CamelCase}Service.getAll()");
            sb.AppendLine("        .pipe(finalize(() => this.loading = false))");
            sb.AppendLine("        .subscribe({");
            sb.AppendLine("          next: (data) => {");

            // Ordenamiento por código o el primer campo string
            var sortField = GetPrimarySortField();
            sb.AppendLine($"            this.{_metadata.PluralCamelCase} = data.sort((a, b) => a.{sortField}.localeCompare(b.{sortField}));");
            sb.AppendLine($"            this.filtered{_metadata.PluralEntityName} = [...this.{_metadata.PluralCamelCase}];");
            sb.AppendLine();
            sb.AppendLine("            if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;");
            sb.AppendLine($"            const totalItems = this.filtered{_metadata.PluralEntityName}.length;");
            sb.AppendLine("            this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);");
            sb.AppendLine();
            sb.AppendLine("            if (this.totalPages === 0) {");
            sb.AppendLine("              this.currentPage = 1;");
            sb.AppendLine("            } else {");
            sb.AppendLine("              if (this.currentPage < 1) this.currentPage = 1;");
            sb.AppendLine("              if (this.currentPage > this.totalPages) this.currentPage = this.totalPages;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            this.applyPagination();");
            sb.AppendLine("          },");
            sb.AppendLine("          error: (err) => {");
            sb.AppendLine($"            this.error = 'Error al cargar {_metadata.PluralCamelCase}';");
            sb.AppendLine("            console.error('Error:', err);");
            sb.AppendLine("          }");
            sb.AppendLine("        });");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine();
        }

        private void AppendSearchAndPaginationMethods(StringBuilder sb)
        {
            // onSearch
            sb.AppendLine("  onSearch(): void {");
            sb.AppendLine("    const term = this.searchTerm.toLowerCase().trim();");
            sb.AppendLine();
            sb.AppendLine($"    if (!term) this.filtered{_metadata.PluralEntityName} = [...this.{_metadata.PluralCamelCase}];");
            sb.AppendLine("    else {");
            sb.Append($"      this.filtered{_metadata.PluralEntityName} = this.{_metadata.PluralCamelCase}.filter(item => ");

            // Generar filtro para los campos relevantes
            var searchableFields = _metadata.Properties
                .Where(p => p.Type == "string" && !p.Name.Contains("Por") && !p.Name.Contains("Fecha"))
                .Take(3)
                .ToList();

            var filterConditions = searchableFields.Select(f => $"item.{ToCamelCase(f.Name)}?.toLowerCase().includes(term)");
            sb.AppendLine(string.Join(" ||\n        ", filterConditions));
            sb.AppendLine("      );");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    this.currentPage = 1;");
            sb.AppendLine("    this.applyPagination();");
            sb.AppendLine("  }");
            sb.AppendLine();

            // applyPagination
            sb.AppendLine("  applyPagination(): void {");
            sb.AppendLine("    if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;");
            sb.AppendLine();
            sb.AppendLine($"    const totalItems = this.filtered{_metadata.PluralEntityName}.length;");
            sb.AppendLine("    this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);");
            sb.AppendLine();
            sb.AppendLine("    if (this.totalPages === 0) {");
            sb.AppendLine("      this.currentPage = 1;");
            sb.AppendLine($"      this.paginated{_metadata.PluralEntityName} = [];");
            sb.AppendLine("      this.startItem = 0;");
            sb.AppendLine("      this.endItem = 0;");
            sb.AppendLine("      return;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);");
            sb.AppendLine();
            sb.AppendLine("    const startIndex = (this.currentPage - 1) * this.pageSize;");
            sb.AppendLine("    const endIndex = Math.min(startIndex + this.pageSize, totalItems);");
            sb.AppendLine();
            sb.AppendLine($"    this.paginated{_metadata.PluralEntityName} = this.filtered{_metadata.PluralEntityName}.slice(startIndex, endIndex);");
            sb.AppendLine();
            sb.AppendLine("    this.startItem = totalItems === 0 ? 0 : startIndex + 1;");
            sb.AppendLine("    this.endItem = endIndex;");
            sb.AppendLine("  }");
            sb.AppendLine();

            // Pagination controls
            sb.AppendLine("  goToPage(page: number): void {");
            sb.AppendLine("    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {");
            sb.AppendLine("      this.currentPage = page;");
            sb.AppendLine("      this.applyPagination();");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  nextPage(): void {");
            sb.AppendLine("    if (this.currentPage < this.totalPages) {");
            sb.AppendLine("      this.currentPage++;");
            sb.AppendLine("      this.applyPagination();");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  previousPage(): void {");
            sb.AppendLine("    if (this.currentPage > 1) {");
            sb.AppendLine("      this.currentPage--;");
            sb.AppendLine("      this.applyPagination();");
            sb.AppendLine("    }");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  getPageNumbers(): number[] {");
            sb.AppendLine("    const pages: number[] = [];");
            sb.AppendLine("    const maxVisiblePages = 5;");
            sb.AppendLine("    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));");
            sb.AppendLine("    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);");
            sb.AppendLine();
            sb.AppendLine("    if (endPage - startPage < maxVisiblePages - 1) {");
            sb.AppendLine("      startPage = Math.max(1, endPage - maxVisiblePages + 1);");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    for (let i = startPage; i <= endPage; i++) pages.push(i);");
            sb.AppendLine("    return pages;");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  onPageSizeChange(newSize?: number | string): void {");
            sb.AppendLine("    const parsed = Number(newSize);");
            sb.AppendLine("    if (!isNaN(parsed) && parsed > 0) this.pageSize = parsed;");
            sb.AppendLine("    else if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;");
            sb.AppendLine();
            sb.AppendLine("    this.currentPage = 1;");
            sb.AppendLine("    this.applyPagination();");
            sb.AppendLine("  }");
        }

        private string GenerateListComponentHtml()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"page-container\">");
            sb.AppendLine("  <div class=\"page-header\">");
            sb.AppendLine("    <div class=\"header-content\">");
            sb.AppendLine($"      <h1 class=\"page-title\">📄 {{{{ '{_metadata.TranslationKey}.title' | translate }}}}</h1>");
            sb.AppendLine($"      <p class=\"page-subtitle\">{{{{ '{_metadata.TranslationKey}.subtitle' | translate }}}}</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"header-actions\">");
            sb.AppendLine("      <button class=\"btn btn-primary\" [routerLink]=\"['nuevo']\">");
            sb.AppendLine("        <span class=\"btn-icon\">➕</span>");
            sb.AppendLine("        {{ 'common.new' | translate }}");
            sb.AppendLine("      </button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div class=\"alert alert-error\" *ngIf=\"error\">");
            sb.AppendLine("    <span class=\"alert-icon\">⚠️</span>");
            sb.AppendLine("    {{ error }}");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div class=\"card\" *ngIf=\"!loading && !error\">");
            sb.AppendLine("    <div class=\"card-header\">");
            sb.AppendLine("      <div class=\"search-container\">");
            sb.AppendLine("        <input type=\"text\" class=\"form-control search-input\"");
            sb.AppendLine("               [(ngModel)]=\"searchTerm\" (input)=\"onSearch()\"");
            sb.AppendLine("               [placeholder]=\"'common.search' | translate\" />");
            sb.AppendLine("        <span class=\"search-icon\">🔍</span>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"page-size-selector\">");
            sb.AppendLine("        <label>{{ 'common.show' | translate }}:</label>");
            sb.AppendLine("        <select [(ngModel)]=\"pageSize\" (ngModelChange)=\"onPageSizeChange($event)\" class=\"form-control\">");
            sb.AppendLine("          <option [value]=\"5\">5</option>");
            sb.AppendLine("          <option [value]=\"10\">10</option>");
            sb.AppendLine("          <option [value]=\"25\">25</option>");
            sb.AppendLine("          <option [value]=\"50\">50</option>");
            sb.AppendLine("          <option [value]=\"100\">100</option>");
            sb.AppendLine("        </select>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine();
            sb.AppendLine("    <div class=\"card-body\">");
            sb.AppendLine("      <div class=\"table-responsive\">");
            sb.AppendLine("        <table class=\"data-table\">");
            sb.AppendLine("          <thead>");
            sb.AppendLine("            <tr>");

            var tableFields = GetTableDisplayFields();
            foreach (var field in tableFields)
            {
                sb.AppendLine($"              <th>{{{{ '{_metadata.TranslationKey}.{ToCamelCase(field.Name)}' | translate }}}}</th>");
            }

            sb.AppendLine("              <th class=\"actions-column\">{{ 'common.actions' | translate }}</th>");
            sb.AppendLine("            </tr>");
            sb.AppendLine("          </thead>");
            sb.AppendLine("          <tbody>");
            sb.AppendLine($"            <tr *ngFor=\"let item of paginated{_metadata.PluralEntityName}\">");

            foreach (var field in tableFields)
            {
                var camelField = ToCamelCase(field.Name);
                if (field.Type == "bool")
                {
                    sb.AppendLine($"              <td>");
                    sb.AppendLine($"                <span class=\"badge\" [class.badge-success]=\"item.{camelField}\" [class.badge-secondary]=\"!item.{camelField}\">");
                    sb.AppendLine($"                  {{{{ item.{camelField} ? ('common.yes' | translate) : ('common.no' | translate) }}}}");
                    sb.AppendLine("                </span>");
                    sb.AppendLine("              </td>");
                }
                else
                {
                    sb.AppendLine($"              <td>{{{{ item.{camelField} }}}}</td>");
                }
            }

            sb.AppendLine("              <td class=\"actions-column\">");
            sb.AppendLine("                <div class=\"action-buttons\">");
            sb.AppendLine("                  <button class=\"btn btn-sm btn-info\" [routerLink]=\"[item.id]\"");
            sb.AppendLine("                          [title]=\"'common.view' | translate\"><span>👁️</span></button>");
            sb.AppendLine("                  <button class=\"btn btn-sm btn-warning\" [routerLink]=\"['editar', item.id]\"");
            sb.AppendLine("                          [title]=\"'common.edit' | translate\"><span>✏️</span></button>");
            sb.AppendLine($"                  <button class=\"btn btn-sm btn-danger\" (click)=\"delete(item.id!, item.{ToCamelCase(GetPrimaryDisplayField())})\"");
            sb.AppendLine("                          [title]=\"'common.delete' | translate\"><span>🗑️</span></button>");
            sb.AppendLine("                </div>");
            sb.AppendLine("              </td>");
            sb.AppendLine("            </tr>");
            sb.AppendLine($"            <tr *ngIf=\"paginated{_metadata.PluralEntityName}.length === 0\">");
            sb.AppendLine($"              <td colspan=\"{tableFields.Count + 1}\" class=\"text-center\">");
            sb.AppendLine("                {{ searchTerm ? ('common.noResults' | translate) : ('common.noData' | translate) }}");
            sb.AppendLine("              </td>");
            sb.AppendLine("            </tr>");
            sb.AppendLine("          </tbody>");
            sb.AppendLine("        </table>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine();
            sb.AppendLine("    <div class=\"card-footer\" *ngIf=\"totalPages > 0\">");
            sb.AppendLine("      <div class=\"pagination-info\">");
            sb.AppendLine("        {{ 'common.showing' | translate }} {{ (currentPage - 1) * pageSize + 1 }} -");
            sb.AppendLine($"        {{{{ Math.min(currentPage * pageSize, filtered{_metadata.PluralEntityName}.length) }}}} {{ 'common.of' | translate }}");
            sb.AppendLine($"        {{{{ filtered{_metadata.PluralEntityName}.length }}}}");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"pagination\">");
            sb.AppendLine("        <button class=\"btn btn-sm btn-secondary\" (click)=\"previousPage()\" [disabled]=\"currentPage === 1\">");
            sb.AppendLine("          ‹ {{ 'common.previous' | translate }}");
            sb.AppendLine("        </button>");
            sb.AppendLine("        <button *ngFor=\"let page of getPageNumbers()\" class=\"btn btn-sm\"");
            sb.AppendLine("                [class.btn-primary]=\"page === currentPage\" [class.btn-secondary]=\"page !== currentPage\"");
            sb.AppendLine("                (click)=\"goToPage(page)\">{{ page }}</button>");
            sb.AppendLine("        <button class=\"btn btn-sm btn-secondary\" (click)=\"nextPage()\" [disabled]=\"currentPage === totalPages\">");
            sb.AppendLine("          {{ 'common.next' | translate }} ›");
            sb.AppendLine("        </button>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div *ngIf=\"loading\" class=\"loading-container\">");
            sb.AppendLine("    <app-loader [translateKey]=\"'common.loading'\" [center]=\"true\" [size]=\"28\" [inline]=\"false\"></app-loader>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        private string GenerateListComponentCss()
        {
            return @".page-container {
      max-width: 1400px;
      margin: 0 auto;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 2rem;
      flex-wrap: wrap;
      gap: 1rem;
    }

    .header-content {
      flex: 1;
    }

    .page-title {
      font-size: 2rem;
      font-weight: 700;
      color: #2c3e50;
      margin: 0 0 0.5rem 0;
    }

    .page-subtitle {
      color: #7f8c8d;
      margin: 0;
      font-size: 1rem;
    }

    .header-actions {
      display: flex;
      gap: 1rem;
    }

    .btn {
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 8px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s;
      text-decoration: none;
    }

    .btn-primary {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
    }

    .btn-primary:hover {
      transform: translateY(-2px);
      box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
    }

    .btn-sm {
      padding: 0.4rem 0.8rem;
      font-size: 0.9rem;
    }

    .btn-info {
      background: #3498db;
      color: white;
    }

    .btn-warning {
      background: #f39c12;
      color: white;
    }

    .btn-danger {
      background: #e74c3c;
      color: white;
    }

    .btn-info:hover,
    .btn-warning:hover,
    .btn-danger:hover {
      transform: translateY(-2px);
      opacity: 0.9;
    }

    .btn-secondary {
      background: #95a5a6;
      color: white;
    }

    .card {
      background: white;
      border-radius: 12px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
      overflow: hidden;
    }

    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1.5rem;
      border-bottom: 1px solid #ecf0f1;
      gap: 1rem;
      flex-wrap: wrap;
    }

    .search-container {
      position: relative;
      flex: 1;
      min-width: 200px;
    }

    .search-input {
      width: 100%;
      padding-right: 2.5rem;
    }

    .search-icon {
      position: absolute;
      right: 0.75rem;
      top: 50%;
      transform: translateY(-50%);
      color: #95a5a6;
    }

    .page-size-selector {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .page-size-selector label {
      margin: 0;
      white-space: nowrap;
    }

    .page-size-selector select {
      width: auto;
    }

    .card-body {
      padding: 0;
    }

    .table-responsive {
      overflow-x: auto;
    }

    .data-table {
      width: 100%;
      border-collapse: collapse;
    }

    .data-table thead {
      background: #f8f9fa;
    }

    .data-table th {
      padding: 1rem;
      text-align: left;
      font-weight: 600;
      color: #2c3e50;
      border-bottom: 2px solid #dee2e6;
    }

    .data-table td {
      padding: 1rem;
      border-bottom: 1px solid #ecf0f1;
      vertical-align: middle;
    }

    .data-table tbody tr:hover {
      background: #f8f9fa;
    }

    .actions-column {
      width: 150px;
      text-align: center;
    }

    .action-buttons {
      display: flex;
      gap: 0.5rem;
      justify-content: center;
    }

    .text-center {
      text-align: center;
    }

    .badge {
      display: inline-block;
      padding: 0.25rem 0.75rem;
      border-radius: 12px;
      font-size: 0.85rem;
      font-weight: 600;
    }

    .badge-info {
      background: #d1ecf1;
      color: #0c5460;
    }

    .badge-success {
      background: #d4edda;
      color: #155724;
    }

    .badge-secondary {
      background: #e2e3e5;
      color: #383d41;
    }

    .card-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 1.5rem;
      background: #f8f9fa;
      border-top: 1px solid #dee2e6;
      flex-wrap: wrap;
      gap: 1rem;
    }

    .pagination {
      display: flex;
      gap: 0.5rem;
    }

    .pagination-info {
      color: #6c757d;
    }

    .alert {
      padding: 1rem 1.5rem;
      margin-bottom: 1.5rem;
      border-radius: 8px;
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .alert-error {
      background: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 300px;
    }

    @media (max-width: 768px) {
      .page-header {
        flex-direction: column;
        align-items: stretch;
      }

      .header-actions {
        justify-content: stretch;
      }

      .header-actions .btn {
        flex: 1;
      }

      .card-header {
        flex-direction: column;
        align-items: stretch;
      }

      .search-container {
        width: 100%;
      }

      .card-footer {
        flex-direction: column;
      }

      .pagination {
        width: 100%;
        justify-content: center;
        flex-wrap: wrap;
      }
    }";
        }

        #endregion

        #region Form Component Generation

        private void GenerateFormComponent()
        {
            var componentDir = Path.Combine(_clientRoot, "pages", _metadata.KebabCase, $"{_metadata.KebabCase}-form");

            if (!_previewMode)
            {
                Directory.CreateDirectory(componentDir);
            }

            var tsContent = GenerateFormComponentTs();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-form.component.ts"), tsContent);

            var htmlContent = GenerateFormComponentHtml();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-form.component.html"), htmlContent);

            var cssContent = GenerateFormComponentCss();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-form.component.css"), cssContent);

            if (_previewMode)
            {
                Console.WriteLine($"  🔍 [PREVIEW] Form Component: {componentDir}");
            }
            else
            {
                Console.WriteLine($"  ✓ Form Component: {componentDir}");
            }
        }

        private string GenerateFormComponentTs()
        {
            var sb = new StringBuilder();

            sb.AppendLine("import { Component, OnInit, OnDestroy } from '@angular/core';");
            sb.AppendLine("import { FormBuilder, FormGroup, Validators } from '@angular/forms';");
            sb.AppendLine("import { Router, ActivatedRoute } from '@angular/router';");
            sb.AppendLine("import { Subscription } from 'rxjs';");
            sb.AppendLine("import { skip } from 'rxjs/operators';");
            sb.AppendLine($"import {{ {_metadata.EntityName}Service, {_metadata.EntityName}DTO }} from '../../../services/{_metadata.KebabCase}.service';");
            sb.AppendLine("import { LanguageService } from '../../../services/language.service';");
            sb.AppendLine("import { TranslationService } from '../../../services/translation.service';");

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"import {{ {fk.RelatedEntity}Service, {fk.RelatedEntity}DTO }} from '../../../services/{ToKebabCase(fk.RelatedEntity)}.service';");
            }

            sb.AppendLine();

            sb.AppendLine("@Component({");
            sb.AppendLine($"  selector: 'app-{_metadata.KebabCase}-form',");
            sb.AppendLine($"  templateUrl: './{_metadata.KebabCase}-form.component.html',");
            sb.AppendLine($"  styleUrls: ['./{_metadata.KebabCase}-form.component.css']");
            sb.AppendLine("})");
            sb.AppendLine($"export class {_metadata.EntityName}FormComponent implements OnInit, OnDestroy {{");

            sb.AppendLine("  form: FormGroup;");
            sb.AppendLine("  isEditMode = false;");
            sb.AppendLine($"  {_metadata.CamelCase}Id: number | null = null;");
            sb.AppendLine("  loading = false;");
            sb.AppendLine("  error: string | null = null;");
            sb.AppendLine();

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"  {ToPluralCamelCase(fk.RelatedEntity)}: {fk.RelatedEntity}DTO[] = [];");
                sb.AppendLine($"  loading{fk.RelatedEntity}s = true;");
            }

            sb.AppendLine();
            sb.AppendLine("  private languageSubscription?: Subscription;");
            sb.AppendLine();

            sb.AppendLine("  constructor(");
            sb.AppendLine("    private fb: FormBuilder,");
            sb.AppendLine($"    private {_metadata.CamelCase}Service: {_metadata.EntityName}Service,");

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"    private {ToCamelCase(fk.RelatedEntity)}Service: {fk.RelatedEntity}Service,");
            }

            sb.AppendLine("    private router: Router,");
            sb.AppendLine("    private route: ActivatedRoute,");
            sb.AppendLine("    private translate: TranslationService,");
            sb.AppendLine("    private languageService: LanguageService");
            sb.AppendLine("  ) {");

            sb.AppendLine("    this.form = this.fb.group({");

            foreach (var prop in _metadata.Properties.Where(p => !p.Name.Contains("Creado") && !p.Name.Contains("Modificado") && p.Name != "Cancelado"))
            {
                var validators = GetFormValidators(prop);
                sb.AppendLine($"      {ToCamelCase(prop.Name)}: [''{(validators.Any() ? ", [" + string.Join(", ", validators) + "]" : "")}],");
            }

            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  ngOnInit(): void {");
            sb.AppendLine("    this.setupFormSubscriptions();");

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"    this.load{fk.RelatedEntity}s();");
            }

            sb.AppendLine();
            sb.AppendLine("    const id = this.route.snapshot.paramMap.get('id');");
            sb.AppendLine("    if (id) {");
            sb.AppendLine("      this.isEditMode = true;");
            sb.AppendLine($"      this.{_metadata.CamelCase}Id = +id;");
            sb.AppendLine($"      this.load{_metadata.EntityName}();");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    this.languageSubscription = this.languageService.currentLanguage$");
            sb.AppendLine("      .pipe(skip(1))");
            sb.AppendLine("      .subscribe(() => {");

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"        this.load{fk.RelatedEntity}s();");
            }

            sb.AppendLine("      });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  ngOnDestroy(): void {");
            sb.AppendLine("    this.languageSubscription?.unsubscribe();");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  private setupFormSubscriptions(): void {");

            var upperCaseFields = _metadata.Properties
                .Where(p => p.Type == "string" &&
                           !p.Name.Contains("Email") &&
                           !p.Name.Contains("Web") &&
                           !p.Name.Contains("Url") &&
                           p.Name != "Codigo")
                .ToList();

            if (upperCaseFields.Any())
            {
                sb.AppendLine($"    const upperFields = [{string.Join(", ", upperCaseFields.Select(f => $"'{ToCamelCase(f.Name)}'"))}];");
                sb.AppendLine("    upperFields.forEach(field => {");
                sb.AppendLine("      this.form.get(field)?.valueChanges.subscribe(value => {");
                sb.AppendLine("        if (value && typeof value === 'string' && value !== value.toUpperCase()) {");
                sb.AppendLine("          this.form.get(field)?.setValue(value.toUpperCase(), { emitEvent: false });");
                sb.AppendLine("        }");
                sb.AppendLine("      });");
                sb.AppendLine("    });");
            }

            sb.AppendLine("  }");
            sb.AppendLine();

            foreach (var fk in _metadata.ForeignKeys)
            {
                sb.AppendLine($"  load{fk.RelatedEntity}s(): void {{");
                sb.AppendLine($"    this.loading{fk.RelatedEntity}s = true;");
                sb.AppendLine($"    this.{ToCamelCase(fk.RelatedEntity)}Service.getAll().subscribe({{");
                sb.AppendLine("      next: (data) => {");
                sb.AppendLine($"        this.{ToPluralCamelCase(fk.RelatedEntity)} = data;");
                sb.AppendLine($"        this.loading{fk.RelatedEntity}s = false;");
                sb.AppendLine("      },");
                sb.AppendLine("      error: (err: any) => {");
                sb.AppendLine($"        this.error = 'Error al cargar {ToPluralCamelCase(fk.RelatedEntity)}';");
                sb.AppendLine($"        this.loading{fk.RelatedEntity}s = false;");
                sb.AppendLine("      }");
                sb.AppendLine("    });");
                sb.AppendLine("  }");
                sb.AppendLine();
            }

            sb.AppendLine($"  load{_metadata.EntityName}(): void {{");
            sb.AppendLine($"    if (!this.{_metadata.CamelCase}Id) return;");
            sb.AppendLine();
            sb.AppendLine("    this.loading = true;");
            sb.AppendLine($"    this.{_metadata.CamelCase}Service.getById(this.{_metadata.CamelCase}Id).subscribe({{");
            sb.AppendLine("      next: (data) => {");
            sb.AppendLine("        this.form.patchValue(data);");
            sb.AppendLine("        this.loading = false;");
            sb.AppendLine("      },");
            sb.AppendLine("      error: (err) => {");
            sb.AppendLine($"        this.error = 'Error al cargar el {_metadata.CamelCase}';");
            sb.AppendLine("        this.loading = false;");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  onSubmit(): void {");
            sb.AppendLine("    if (this.form.invalid) {");
            sb.AppendLine("      Object.keys(this.form.controls).forEach(key => {");
            sb.AppendLine("        this.form.get(key)?.markAsTouched();");
            sb.AppendLine("      });");
            sb.AppendLine("      return;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    this.loading = true;");
            sb.AppendLine("    this.error = null;");
            sb.AppendLine();
            sb.AppendLine($"    const formData: {_metadata.EntityName}DTO = this.form.getRawValue();");
            sb.AppendLine();
            sb.AppendLine("    const request = this.isEditMode");
            sb.AppendLine($"      ? this.{_metadata.CamelCase}Service.update(this.{_metadata.CamelCase}Id!, formData)");
            sb.AppendLine($"      : this.{_metadata.CamelCase}Service.create(formData);");
            sb.AppendLine();
            sb.AppendLine("    request.subscribe({");
            sb.AppendLine("      next: () => {");
            sb.AppendLine($"        this.router.navigate(['/nomencladores/{_metadata.KebabCase}']);");
            sb.AppendLine("      },");
            sb.AppendLine("      error: (err) => {");
            sb.AppendLine("        this.loading = false;");
            sb.AppendLine("        this.error = this.isEditMode ? 'Error al actualizar' : 'Error al crear';");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  cancel(): void {");
            sb.AppendLine($"    this.router.navigate(['/nomencladores/{_metadata.KebabCase}']);");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  getErrorMessage(fieldName: string): string {");
            sb.AppendLine("    const control = this.form.get(fieldName);");
            sb.AppendLine("    if (!control || !control.errors) return '';");
            sb.AppendLine();
            sb.AppendLine("    if (control.errors['required']) return 'Campo requerido';");
            sb.AppendLine("    if (control.errors['maxlength']) {");
            sb.AppendLine("      const max = control.errors['maxlength'].requiredLength;");
            sb.AppendLine("      return `Máximo ${max} caracteres`;");
            sb.AppendLine("    }");
            sb.AppendLine("    if (control.errors['pattern']) return 'Formato inválido';");
            sb.AppendLine("    return 'Campo inválido';");
            sb.AppendLine("  }");

            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateFormComponentHtml()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"page-container\">");
            sb.AppendLine("  <div class=\"page-header\">");
            sb.AppendLine("    <h1 class=\"page-title\">");
            sb.AppendLine($"      {{{{ isEditMode ? ('{_metadata.TranslationKey}.editTitle' | translate) : ('{_metadata.TranslationKey}.newTitle' | translate) }}}}");
            sb.AppendLine("    </h1>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div class=\"alert alert-error\" *ngIf=\"error\">");
            sb.AppendLine("    <span class=\"alert-icon\">⚠️</span>");
            sb.AppendLine("    {{ error }}");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div class=\"card\">");
            sb.AppendLine("    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" class=\"form-container\">");

            foreach (var prop in _metadata.Properties.Where(p =>
                !p.Name.Contains("Creado") &&
                !p.Name.Contains("Modificado") &&
                p.Name != "Cancelado" &&
                p.Name != "Id"))
            {
                var camelName = ToCamelCase(prop.Name);
                var isForeignKey = _metadata.ForeignKeys.Any(fk => fk.PropertyName == prop.Name);

                sb.AppendLine("      <div class=\"form-group\">");
                sb.AppendLine($"        <label for=\"{camelName}\" class=\"form-label\">");
                sb.AppendLine($"          {{{{ '{_metadata.TranslationKey}.{camelName}' | translate }}}}");

                var isRequired = !prop.IsNullable;
                if (isRequired) sb.Append(" *");

                sb.AppendLine("        </label>");

                if (isForeignKey)
                {
                    var fk = _metadata.ForeignKeys.First(f => f.PropertyName == prop.Name);
                    sb.AppendLine($"        <select id=\"{camelName}\" formControlName=\"{camelName}\" class=\"form-control\"");
                    sb.AppendLine($"                [class.is-invalid]=\"form.get('{camelName}')?.invalid && form.get('{camelName}')?.touched\"");
                    sb.AppendLine($"                [disabled]=\"loading{fk.RelatedEntity}s\">");
                    sb.AppendLine("          <option [value]=\"null\">{{ 'common.select' | translate }}</option>");
                    sb.AppendLine($"          <option *ngFor=\"let item of {ToPluralCamelCase(fk.RelatedEntity)}\" [value]=\"item.id\">");
                    sb.AppendLine("            {{ item.codigo }} - {{ item.descripcion }}");
                    sb.AppendLine("          </option>");
                    sb.AppendLine("        </select>");
                }
                else if (prop.Type == "bool")
                {
                    sb.AppendLine("        <div class=\"checkbox-wrapper\">");
                    sb.AppendLine($"          <input type=\"checkbox\" id=\"{camelName}\" formControlName=\"{camelName}\" />");
                    sb.AppendLine($"          <label for=\"{camelName}\">{{{{ '{_metadata.TranslationKey}.{camelName}Help' | translate }}}}</label>");
                    sb.AppendLine("        </div>");
                }
                else if (prop.Type == "DateTime")
                {
                    sb.AppendLine($"        <input type=\"date\" id=\"{camelName}\" formControlName=\"{camelName}\" class=\"form-control\"");
                    sb.AppendLine($"               [class.is-invalid]=\"form.get('{camelName}')?.invalid && form.get('{camelName}')?.touched\" />");
                }
                else if (prop.Type == "int" || prop.Type == "decimal" || prop.Type == "double")
                {
                    sb.AppendLine($"        <input type=\"number\" id=\"{camelName}\" formControlName=\"{camelName}\" class=\"form-control\"");
                    sb.AppendLine($"               [class.is-invalid]=\"form.get('{camelName}')?.invalid && form.get('{camelName}')?.touched\" />");
                }
                else
                {
                    var maxLength = prop.MaxLength > 0 ? $" maxlength=\"{prop.MaxLength}\"" : "";
                    sb.AppendLine($"        <input type=\"text\" id=\"{camelName}\" formControlName=\"{camelName}\" class=\"form-control\"");
                    sb.AppendLine($"               [class.is-invalid]=\"form.get('{camelName}')?.invalid && form.get('{camelName}')?.touched\"{maxLength} />");
                }

                sb.AppendLine($"        <div class=\"invalid-feedback\" *ngIf=\"form.get('{camelName}')?.invalid && form.get('{camelName}')?.touched\">");
                sb.AppendLine($"          {{{{ getErrorMessage('{camelName}') }}}}");
                sb.AppendLine("        </div>");
                sb.AppendLine("      </div>");
                sb.AppendLine();
            }

            sb.AppendLine("      <div class=\"form-actions\">");
            sb.AppendLine("        <button type=\"button\" class=\"btn btn-secondary\" (click)=\"cancel()\" [disabled]=\"loading\">");
            sb.AppendLine("          ❌ {{ 'common.cancel' | translate }}");
            sb.AppendLine("        </button>");
            sb.AppendLine("        <button type=\"submit\" class=\"btn btn-primary\" [disabled]=\"loading || form.invalid\">");
            sb.AppendLine("          <span *ngIf=\"!loading\">💾 {{ 'common.save' | translate }}</span>");
            sb.AppendLine("          <span *ngIf=\"loading\">⏳ {{ 'common.saving' | translate }}</span>");
            sb.AppendLine("        </button>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </form>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        private string GenerateFormComponentCss()
        {
            return @".page-container {
      max-width: 800px;
      margin: 0 auto;
      padding: 2rem 1rem;
    }

    .page-header {
      margin-bottom: 2rem;
    }

    .page-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: #2c3e50;
      margin: 0;
    }

    .card {
      background: white;
      border-radius: 12px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
      padding: 2rem;
    }

    .form-container {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }

    .form-label {
      font-weight: 600;
      color: #2c3e50;
      font-size: 0.95rem;
    }

    .form-control {
      padding: 0.75rem;
      border: 1px solid #ced4da;
      border-radius: 6px;
      font-size: 1rem;
      transition: border-color 0.3s, box-shadow 0.3s;
    }

    .form-control:focus {
      outline: none;
      border-color: #667eea;
      box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
    }

    .form-control.is-invalid {
      border-color: #dc3545;
    }

    .form-control:disabled {
      background-color: #e9ecef;
      cursor: not-allowed;
    }

    .invalid-feedback {
      color: #dc3545;
      font-size: 0.875rem;
      margin-top: 0.25rem;
    }

    .checkbox-wrapper {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .checkbox-wrapper input[type='checkbox'] {
      width: 20px;
      height: 20px;
      cursor: pointer;
    }

    .form-actions {
      display: flex;
      gap: 1rem;
      justify-content: flex-end;
      margin-top: 1rem;
      padding-top: 1.5rem;
      border-top: 1px solid #e9ecef;
    }

    .btn {
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 8px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s;
      text-decoration: none;
      font-size: 1rem;
    }

    .btn:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }

    .btn-primary {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
    }

    .btn-primary:hover:not(:disabled) {
      transform: translateY(-2px);
      box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
    }

    .btn-secondary {
      background: #6c757d;
      color: white;
    }

    .btn-secondary:hover:not(:disabled) {
      background: #5a6268;
      transform: translateY(-2px);
    }

    .alert {
      padding: 1rem 1.5rem;
      margin-bottom: 1.5rem;
      border-radius: 8px;
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .alert-error {
      background: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }

    @media (max-width: 768px) {
      .page-container {
        padding: 1rem 0.5rem;
      }

      .card {
        padding: 1.5rem 1rem;
      }

      .form-actions {
        flex-direction: column-reverse;
      }

      .form-actions .btn {
        width: 100%;
        justify-content: center;
      }
    }";
        }

        #endregion

        #region Detail Component Generation

        private void GenerateDetailComponent()
        {
            var componentDir = Path.Combine(_clientRoot, "pages", _metadata.KebabCase, $"{_metadata.KebabCase}-detail");

            if (!_previewMode)
            {
                Directory.CreateDirectory(componentDir);
            }

            var tsContent = GenerateDetailComponentTs();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-detail.component.ts"), tsContent);

            var htmlContent = GenerateDetailComponentHtml();
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-detail.component.html"), htmlContent);

            var cssContent = "/* Estilos compartidos con form */\n@import '../" + _metadata.KebabCase + "-form/" + _metadata.KebabCase + "-form.component.css';";
            WriteFile(Path.Combine(componentDir, $"{_metadata.KebabCase}-detail.component.css"), cssContent);

            if (_previewMode)
            {
                Console.WriteLine($"  🔍 [PREVIEW] Detail Component: {componentDir}");
            }
            else
            {
                Console.WriteLine($"  ✓ Detail Component: {componentDir}");
            }
        }

        private string GenerateDetailComponentTs()
        {
            var sb = new StringBuilder();

            sb.AppendLine("import { Component, OnInit, OnDestroy } from '@angular/core';");
            sb.AppendLine("import { ActivatedRoute, Router } from '@angular/router';");
            sb.AppendLine("import { Subscription } from 'rxjs';");
            sb.AppendLine("import { skip } from 'rxjs/operators';");
            sb.AppendLine($"import {{ {_metadata.EntityName}Service, {_metadata.EntityName}DTO }} from '../../../services/{_metadata.KebabCase}.service';");
            sb.AppendLine("import { LanguageService } from '../../../services/language.service';");
            sb.AppendLine();

            sb.AppendLine("@Component({");
            sb.AppendLine($"  selector: 'app-{_metadata.KebabCase}-detail',");
            sb.AppendLine($"  templateUrl: './{_metadata.KebabCase}-detail.component.html',");
            sb.AppendLine($"  styleUrls: ['./{_metadata.KebabCase}-detail.component.css']");
            sb.AppendLine("})");
            sb.AppendLine($"export class {_metadata.EntityName}DetailComponent implements OnInit, OnDestroy {{");
            sb.AppendLine($"  {_metadata.CamelCase}: {_metadata.EntityName}DTO | null = null;");
            sb.AppendLine("  id: number | null = null;");
            sb.AppendLine("  loading = true;");
            sb.AppendLine("  error: string | null = null;");
            sb.AppendLine();
            sb.AppendLine("  private languageSubscription?: Subscription;");
            sb.AppendLine();

            sb.AppendLine("  constructor(");
            sb.AppendLine($"    private {_metadata.CamelCase}Service: {_metadata.EntityName}Service,");
            sb.AppendLine("    private route: ActivatedRoute,");
            sb.AppendLine("    private router: Router,");
            sb.AppendLine("    private languageService: LanguageService");
            sb.AppendLine("  ) {}");
            sb.AppendLine();

            sb.AppendLine("  ngOnInit(): void {");
            sb.AppendLine("    const idParam = this.route.snapshot.paramMap.get('id');");
            sb.AppendLine("    if (idParam) {");
            sb.AppendLine("      this.id = +idParam;");
            sb.AppendLine($"      this.load{_metadata.EntityName}();");
            sb.AppendLine("    } else {");
            sb.AppendLine("      this.error = 'ID no válido';");
            sb.AppendLine("      this.loading = false;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    this.languageSubscription = this.languageService.currentLanguage$");
            sb.AppendLine("      .pipe(skip(1))");
            sb.AppendLine("      .subscribe(() => {");
            sb.AppendLine($"        this.load{_metadata.EntityName}();");
            sb.AppendLine("      });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  ngOnDestroy(): void {");
            sb.AppendLine("    this.languageSubscription?.unsubscribe();");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine($"  load{_metadata.EntityName}(): void {{");
            sb.AppendLine("    if (!this.id) return;");
            sb.AppendLine();
            sb.AppendLine("    this.loading = true;");
            sb.AppendLine($"    this.{_metadata.CamelCase}Service.getById(this.id).subscribe({{");
            sb.AppendLine("      next: (data) => {");
            sb.AppendLine($"        this.{_metadata.CamelCase} = data;");
            sb.AppendLine("        this.loading = false;");
            sb.AppendLine("      },");
            sb.AppendLine("      error: (err) => {");
            sb.AppendLine($"        this.error = 'Error al cargar el {_metadata.CamelCase}';");
            sb.AppendLine("        this.loading = false;");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine();

            sb.AppendLine("  goBack(): void {");
            sb.AppendLine($"    this.router.navigate(['/nomencladores/{_metadata.KebabCase}']);");
            sb.AppendLine("  }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateDetailComponentHtml()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"page-container\">");
            sb.AppendLine("  <div class=\"page-header\">");
            sb.AppendLine("    <div class=\"header-content\">");
            sb.AppendLine("      <h1 class=\"page-title\">");
            sb.AppendLine($"        📊 {{{{ '{_metadata.TranslationKey}.detailTitle' | translate }}}}");
            sb.AppendLine("      </h1>");
            sb.AppendLine($"      <p class=\"page-subtitle\">{{{{ '{_metadata.TranslationKey}.detailSubtitle' | translate }}}}</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"header-actions\">");
            sb.AppendLine($"      <button class=\"btn btn-warning\" [routerLink]=\"['/nomencladores/{_metadata.KebabCase}/editar', id]\">");
            sb.AppendLine("        <span class=\"btn-icon\">✏️</span>");
            sb.AppendLine("        {{ 'common.edit' | translate }}");
            sb.AppendLine("      </button>");
            sb.AppendLine("      <button class=\"btn btn-secondary\" (click)=\"goBack()\">");
            sb.AppendLine("        <span class=\"btn-icon\">↩️</span>");
            sb.AppendLine("        {{ 'common.back' | translate }}");
            sb.AppendLine("      </button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div class=\"alert alert-error\" *ngIf=\"error\">");
            sb.AppendLine("    <span class=\"alert-icon\">⚠️</span>");
            sb.AppendLine("    {{ error }}");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine($"  <div class=\"card\" *ngIf=\"!loading && {_metadata.CamelCase}\">");
            sb.AppendLine("    <div class=\"card-header\">");
            sb.AppendLine("      📊 {{ 'common.information' | translate }}");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"card-body\">");
            sb.AppendLine("      <div class=\"info-section\">");
            sb.AppendLine("        <h3 class=\"section-title\">");
            sb.AppendLine($"          🔑 {{{{ '{_metadata.TranslationKey}.mainInfo' | translate }}}}");
            sb.AppendLine("        </h3>");
            sb.AppendLine("        <div class=\"info-grid\">");

            foreach (var prop in _metadata.Properties.Where(p =>
                !p.Name.Contains("Creado") &&
                !p.Name.Contains("Modificado") &&
                p.Name != "Cancelado"))
            {
                var camelName = ToCamelCase(prop.Name);
                sb.AppendLine("          <div class=\"info-item\">");
                sb.AppendLine($"            <span class=\"info-label\">{{{{ '{_metadata.TranslationKey}.{camelName}' | translate }}}}</span>");

                if (prop.Type == "bool")
                {
                    sb.AppendLine($"            <span class=\"badge\" [class.badge-success]=\"{_metadata.CamelCase}.{camelName}\" [class.badge-secondary]=\"!{_metadata.CamelCase}.{camelName}\">");
                    sb.AppendLine($"              {{{{ {_metadata.CamelCase}.{camelName} ? ('common.yes' | translate) : ('common.no' | translate) }}}}");
                    sb.AppendLine("            </span>");
                }
                else if (prop.Type == "DateTime")
                {
                    sb.AppendLine($"            <span class=\"info-value\">{{{{ {_metadata.CamelCase}.{camelName} | date:'dd/MM/yyyy' }}}}</span>");
                }
                else
                {
                    sb.AppendLine($"            <span class=\"info-value\">{{{{ {_metadata.CamelCase}.{camelName} }}}}</span>");
                }

                sb.AppendLine("          </div>");
            }

            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine();
            sb.AppendLine("  <div *ngIf=\"loading\" class=\"loading-container\">");
            sb.AppendLine("    <app-loader [translateKey]=\"'common.loading'\" [center]=\"true\" [size]=\"28\" [inline]=\"false\"></app-loader>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        #endregion

        #region Helper Methods

        private string MapToTypeScriptType(string csType)
        {
            return csType.ToLower() switch
            {
                "string" => "string",
                "int" => "number",
                "long" => "number",
                "decimal" => "number",
                "double" => "number",
                "float" => "number",
                "bool" => "boolean",
                "boolean" => "boolean",
                "datetime" => "Date",
                "guid" => "string",
                _ => "any"
            };
        }

        private string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToLower(str[0]) + str.Substring(1);
        }

        private string ToKebabCase(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }

        private string ToPluralCamelCase(string str)
        {
            var camel = ToCamelCase(str);
            return camel.EndsWith("s") ? camel : camel + "s";
        }

        private string GetPrimaryDisplayField()
        {
            var descField = _metadata.Properties.FirstOrDefault(p => p.Name == "Descripcion");
            if (descField != null) return ToCamelCase(descField.Name);

            var nameField = _metadata.Properties.FirstOrDefault(p => p.Name.Contains("Nombre"));
            if (nameField != null) return ToCamelCase(nameField.Name);

            var codigoField = _metadata.Properties.FirstOrDefault(p => p.Name == "Codigo");
            if (codigoField != null) return ToCamelCase(codigoField.Name);

            return "id";
        }

        private string GetPrimarySortField()
        {
            var codigoField = _metadata.Properties.FirstOrDefault(p => p.Name == "Codigo");
            if (codigoField != null) return ToCamelCase(codigoField.Name);

            var descField = _metadata.Properties.FirstOrDefault(p => p.Name == "Descripcion");
            if (descField != null) return ToCamelCase(descField.Name);

            return "id";
        }

        private List<PropertyMetadata> GetTableDisplayFields()
        {
            var fields = new List<PropertyMetadata>();

            var codigo = _metadata.Properties.FirstOrDefault(p => p.Name == "Codigo");
            if (codigo != null) fields.Add(codigo);

            var descripcion = _metadata.Properties.FirstOrDefault(p => p.Name == "Descripcion");
            if (descripcion != null) fields.Add(descripcion);

            fields.AddRange(_metadata.Properties
                .Where(p => (p.Name.EndsWith("Id") && _metadata.ForeignKeys.Any(fk => fk.PropertyName == p.Name)) || p.Type == "bool")
                .Take(2));

            if (fields.Count < 4)
            {
                fields.AddRange(_metadata.Properties
                    .Where(p => p.Type == "string" && !fields.Contains(p) && !p.Name.Contains("Creado") && !p.Name.Contains("Modificado"))
                    .Take(4 - fields.Count));
            }

            return fields.Take(4).ToList();
        }

        private List<string> GetFormValidators(PropertyMetadata prop)
        {
            var validators = new List<string>();

            if (!prop.IsNullable)
            {
                validators.Add("Validators.required");
            }

            if (prop.Type == "string" && prop.MaxLength > 0)
            {
                validators.Add($"Validators.maxLength({prop.MaxLength})");
            }

            if (prop.Name == "Email" || prop.Name.Contains("Email"))
            {
                validators.Add("Validators.email");
            }

            if (prop.Name == "Codigo")
            {
                if (prop.MaxLength == 2)
                    validators.Add(@"Validators.pattern(/^\d{2}$/)");
                else if (prop.MaxLength == 5)
                    validators.Add(@"Validators.pattern(/^\d{5}$/)");
                else if (prop.MaxLength == 8)
                    validators.Add(@"Validators.pattern(/^\d{8}$/)");
            }

            return validators;
        }

        private void WriteFile(string path, string content)
        {
            if (_previewMode)
            {
                return;
            }

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(path, content);
        }

        #endregion
    }
}