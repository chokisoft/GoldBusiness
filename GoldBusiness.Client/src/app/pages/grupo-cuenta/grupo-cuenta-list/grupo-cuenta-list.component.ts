import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { GrupoCuentaService, GrupoCuentaDTO } from '../../../services/grupo-cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-grupo-cuenta-list',
  templateUrl: './grupo-cuenta-list.component.html',
  styleUrls: ['./grupo-cuenta-list.component.css']
})
export class GrupoCuentaListComponent implements OnInit, OnDestroy {
  grupoCuentas: GrupoCuentaDTO[] = [];
  filteredGrupoCuentas: GrupoCuentaDTO[] = [];
  paginatedGrupoCuentas: GrupoCuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  // Valores calculados para UI
  startItem: number = 0;
  endItem: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  private languageSubscription?: Subscription;

  constructor(
    private grupoCuentaService: GrupoCuentaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 GrupoCuentaList: Idioma cambiado, recargando datos...');
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    Promise.resolve().then(() => {
      this.grupoCuentaService.getAll()
        .pipe(finalize(() => this.loading = false))
        .subscribe({
          next: (data) => {
            this.grupoCuentas = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
            this.filteredGrupoCuentas = [...this.grupoCuentas];

            if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
            const totalItems = this.filteredGrupoCuentas.length;
            this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

            if (this.totalPages === 0) {
              this.currentPage = 1;
            } else {
              if (this.currentPage < 1) this.currentPage = 1;
              if (this.currentPage > this.totalPages) this.currentPage = this.totalPages;
            }

            this.applyPagination();
          },
          error: (err) => {
            this.error = 'Error al cargar los grupos de cuenta';
            console.error('Error:', err);
          }
        });
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) this.filteredGrupoCuentas = [...this.grupoCuentas];
    else {
      this.filteredGrupoCuentas = this.grupoCuentas.filter(grupo =>
        grupo.codigo.toLowerCase().includes(term) ||
        grupo.descripcion.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;

    const totalItems = this.filteredGrupoCuentas.length;
    this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

    if (this.totalPages === 0) {
      this.currentPage = 1;
      this.paginatedGrupoCuentas = [];
      this.startItem = 0;
      this.endItem = 0;
      return;
    }

    this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = Math.min(startIndex + this.pageSize, totalItems);

    this.paginatedGrupoCuentas = this.filteredGrupoCuentas.slice(startIndex, endIndex);

    this.startItem = totalItems === 0 ? 0 : startIndex + 1;
    this.endItem = endIndex;
  }

  goToPage(page: number): void {
    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {
      this.currentPage = page;
      this.applyPagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyPagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyPagination();
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

    for (let i = startPage; i <= endPage; i++) pages.push(i);

    return pages;
  }

  onPageSizeChange(newSize?: number | string): void {
    const parsed = Number(newSize);
    if (!isNaN(parsed) && parsed > 0) this.pageSize = parsed;
    else if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;

    this.currentPage = 1;
    this.applyPagination();
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar el grupo de cuenta "${descripcion}"?`)) {
      this.grupoCuentaService.delete(id).subscribe({
        next: () => this.loadData(),
        error: (err) => {
          this.error = 'Error al eliminar el grupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
