import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { finalize, skip } from 'rxjs/operators';
import { LanguageService } from '../../../services/language.service';
import { ProductoDTO, ProductoService } from '../../../services/producto.service';

@Component({
  selector: 'app-producto-list',
  templateUrl: './producto-list.component.html',
  styleUrl: './producto-list.component.css'
})
export class ProductoListComponent implements OnInit, OnDestroy {
  productos: ProductoDTO[] = [];
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
    private productoService: ProductoService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData(true);
    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => this.loadData(true));
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

    this.productoService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          this.productos = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          this.error = `Error al cargar los productos: ${err?.message ?? err.statusText}`;
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
    this.pageSize = !isNaN(parsed) && parsed > 0 ? parsed : 10;
    this.currentPage = 1;
    this.loadData(false);
  }

  delete(id: number, nombre: string): void {
    if (confirm(`¿Estás seguro de eliminar el producto "${nombre}"?`)) {
      this.productoService.delete(id).subscribe({
        next: () => {
          if (this.productos.length === 1 && this.currentPage > 1) this.currentPage--;
          this.loadData(false);
        },
        error: () => this.error = 'Error al eliminar el producto'
      });
    }
  }

}
