import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-cuenta-detail',
  templateUrl: './cuenta-detail.component.html',
  styleUrls: ['./cuenta-detail.component.css']
})
export class CuentaDetailComponent implements OnInit {
  cuenta: CuentaDTO | null = null;
  loading = true;
  error: string | null = null;
  cuentaId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cuentaService: CuentaService
  ) {
    this.cuentaId = +this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.loadCuenta();
  }

  loadCuenta(): void {
    this.loading = true;
    this.error = null;

    this.cuentaService.getById(this.cuentaId).subscribe({
      next: (data) => {
        this.cuenta = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar la cuenta';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  goToEdit(): void {
    this.router.navigate(['/nomencladores/cuenta/editar', this.cuentaId]);
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/cuenta']);
  }

  delete(): void {
    if (!confirm(`¿Está seguro que desea eliminar la cuenta "${this.cuenta?.descripcion}"?`)) {
      return;
    }

    this.cuentaService.delete(this.cuentaId).subscribe({
      next: () => {
        this.router.navigate(['/nomencladores/cuenta']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Error al eliminar la cuenta';
        console.error('Error:', err);
      }
    });
  }
}
