import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-subgrupo-cuenta-list',
  templateUrl: './subgrupo-cuenta-list.component.html',
  styleUrls: ['./subgrupo-cuenta-list.component.css']
})
export class SubGrupoCuentaListComponent implements OnInit, OnDestroy {
  subGrupoCuentas: SubGrupoCuentaDTO[] = [];
  filteredSubGrupoCuentas: SubGrupoCuentaDTO[] = [];
  paginatedSubGrupoCuentas: SubGrupoCuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  private languageSubscription?: Subscription;

  constructor(
    private subGrupoCuentaService: SubGrupoCuentaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SubGrupoCuentaList: Idioma cambiado, recargando datos...');
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.subGrupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.subGrupoCuentas = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
        this.filteredSubGrupoCuentas = [...this.subGrupoCuentas];
        this.applyPagination();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los subgrupos de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) {
      this.filteredSubGrupoCuentas = [...this.subGrupoCuentas];
    } else {
      this.filteredSubGrupoCuentas = this.subGrupoCuentas.filter(subgrupo =>
        subgrupo.codigo.toLowerCase().includes(term) ||
        subgrupo.descripcion.toLowerCase().includes(term) ||
        subgrupo.grupoCuentaCodigo?.toLowerCase().includes(term) ||
        subgrupo.grupoCuentaDescripcion?.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    this.totalPages = Math.ceil(this.filteredSubGrupoCuentas.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedSubGrupoCuentas = this.filteredSubGrupoCuentas.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
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

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.applyPagination();
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar el subgrupo de cuenta "${descripcion}"?`)) {
      this.subGrupoCuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar el subgrupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
