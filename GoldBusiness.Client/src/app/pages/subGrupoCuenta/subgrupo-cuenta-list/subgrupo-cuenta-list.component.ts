import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/sub-grupo-cuenta.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-subgrupo-cuenta-list',
  templateUrl: './subgrupo-cuenta-list.component.html',
  styleUrls: ['./subgrupo-cuenta-list.component.css']
})
export class SubGrupoCuentaListComponent implements OnInit, OnDestroy {
  subGrupoCuentas: SubGrupoCuentaDTO[] = [];
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
    private subGrupoCuentaService: SubGrupoCuentaService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData(true);

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 SubGrupoCuentaList: Idioma cambiado, recargando...');
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
    
    this.subGrupoCuentaService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /SubGrupoCuenta/paged response:', response);
          this.subGrupoCuentas = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error:', err);
          this.error = `Error al cargar los subgrupos de cuenta: ${err?.message ?? err.statusText}`;
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
    for (let i = startPage; i <= endPage; i++) pages.push(i);
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
    if (confirm(`¿Estás seguro de eliminar el subgrupo de cuenta "${descripcion}"?`)) {
      this.subGrupoCuentaService.delete(id).subscribe({
        next: () => {
          if (this.subGrupoCuentas.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData(false);
        },
        error: (err) => {
          this.error = 'Error al eliminar el subgrupo de cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
