import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { PaisService, PaisDTO } from '../../../services/pais.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-pais-list',
  templateUrl: './pais-list.component.html',
  styleUrls: ['./pais-list.component.css']
})
export class PaisListComponent implements OnInit, OnDestroy {
  paises: PaisDTO[] = [];
  filteredPaises: PaisDTO[] = [];
  paginatedPaises: PaisDTO[] = [];
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
    private paisService: PaisService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 PaisList: Idioma cambiado, recargando datos...');
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
      this.paisService.getAll()
        .pipe(finalize(() => this.loading = false))
        .subscribe({
          next: (data) => {
            this.paises = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
            this.filteredPaises = [...this.paises];

            if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
            const totalItems = this.filteredPaises.length;
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
            this.error = 'Error al cargar los países';
            console.error('Error:', err);
          }
        });
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();

    if (!term) this.filteredPaises = [...this.paises];
    else {
      this.filteredPaises = this.paises.filter(grupo =>
        grupo.codigo.toLowerCase().includes(term) ||
        (grupo.descripcion || '').toLowerCase().includes(term)
      );
    }

    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;

    const totalItems = this.filteredPaises.length;
    this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

    if (this.totalPages === 0) {
      this.currentPage = 1;
      this.paginatedPaises = [];
      this.startItem = 0;
      this.endItem = 0;
      return;
    }

    this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = Math.min(startIndex + this.pageSize, totalItems);

    this.paginatedPaises = this.filteredPaises.slice(startIndex, endIndex);

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
    if (confirm(`¿Estás seguro de eliminar el país "${descripcion}"?`)) {
      this.paisService.delete(id).subscribe({
        next: () => this.loadData(),
        error: (err) => {
          this.error = 'Error al eliminar el país';
          console.error('Error:', err);
        }
      });
    }
  }
}
