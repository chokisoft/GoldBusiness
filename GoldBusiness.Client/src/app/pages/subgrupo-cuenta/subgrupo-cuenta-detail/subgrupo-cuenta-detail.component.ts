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
  loading = true;
  error: string | null = null;
  subgrupoId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subGrupoCuentaService: SubGrupoCuentaService
  ) {
    this.subgrupoId = +this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadSubGrupoCuenta();
  }

  loadSubGrupoCuenta(): void {
    this.loading = true;
    this.error = null;

    this.subGrupoCuentaService.getById(this.subgrupoId).subscribe({
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
    this.router.navigate(['/nomencladores/subgrupo-cuenta/editar', this.subgrupoId]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/subgrupo-cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar el subgrupo "${this.subgrupo?.descripcion}"?`)) {
      return;
    }

    this.subGrupoCuentaService.delete(this.subgrupoId).subscribe({
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
