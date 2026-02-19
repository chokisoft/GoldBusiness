import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubGrupoCuentaService, SubGrupoCuentaDTO } from '../../../services/subgrupo-cuenta.service';

@Component({
  selector: 'app-subgrupo-cuenta-detail',
  templateUrl: './subgrupo-cuenta-detail.component.html',
  styleUrls: ['./subgrupo-cuenta-detail.component.css']
})
export class SubGrupoCuentaDetailComponent implements OnInit {
  subgrupo: SubGrupoCuentaDTO | null = null;
  id: number | null = null; // ✅ DEBE EXISTIR
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subGrupoCuentaService: SubGrupoCuentaService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.loadSubGrupoCuenta();
    }
  }

  loadSubGrupoCuenta(): void {
    this.loading = true;
    this.error = null;

    this.subGrupoCuentaService.getById(this.id!).subscribe({
      next: (data) => {
        this.subgrupo = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el subgrupo de cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta/editar', this.id]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar el subgrupo "${this.subgrupo?.descripcion}"?`)) {
      return;
    }

    this.subGrupoCuentaService.delete(this.id!).subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/subgrupo-cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al eliminar el subgrupo de cuenta';
        console.error('Error:', err);
      }
    });
  }
}
