import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { LocalidadService, LocalidadDTO } from '../../../services/localidad.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-localidad-list',
  templateUrl: './localidad-list.component.html',
  styleUrls: ['./localidad-list.component.css']
})
export class LocalidadListComponent implements OnInit, OnDestroy {
  localidades: LocalidadDTO[] = [];
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
    private localidadService: LocalidadService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData(true);

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 LocalidadList: Idioma cambiado, recargando datos...');
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
    
    this.localidadService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /Localidad/paged response:', response);
          this.localidades = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error:', err);
          this.error = `Error al cargar las localidades: ${err?.message ?? err.statusText}`;
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
    if (!isNaN(parsed) && parsed > 0) {
      this.pageSize = parsed;
    } else if (!this.pageSize || this.pageSize <= 0) {
      this.pageSize = 10;
    }

    this.currentPage = 1;
    this.loadData(false);
  }

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Estás seguro de eliminar la localidad "${descripcion}"?`)) {
      this.localidadService.delete(id).subscribe({
        next: () => {
          // Si eliminamos el último item de la página actual y no es la primera, retroceder
          if (this.localidades.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData(false);
        },
        error: (err) => {
          this.error = 'Error al eliminar la localidad';
          console.error('Error:', err);
        }
      });
    }
  }
}
