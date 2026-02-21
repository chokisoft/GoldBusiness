import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
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

    this.grupoCuentaService.getAll().subscribe({
      next: (data) => {
        this.grupoCuentas = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
        this.filteredGrupoCuentas = [...this.grupoCuentas];
        this.applyPagination();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los grupos de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) {
      this.filteredGrupoCuentas = [...this.grupoCuentas];
    } else {
      this.filteredGrupoCuentas = this.grupoCuentas.filter(grupo =>
        grupo.codigo.toLowerCase().includes(term) ||
        grupo.descripcion.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    this.totalPages = Math.ceil(this.filteredGrupoCuentas.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedGrupoCuentas = this.filteredGrupoCuentas.slice(startIndex, endIndex);
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
    if (confirm(`¿Está seguro de eliminar el grupo de cuenta "${descripcion}"?`)) {
      this.grupoCuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar el grupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
