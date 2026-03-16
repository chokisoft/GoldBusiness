import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ClienteDTO, ClienteService } from '../../../services/cliente.service';

@Component({
  selector: 'app-cliente-detail',
  templateUrl: './cliente-detail.component.html',
  styleUrls: ['./cliente-detail.component.css']
})
export class ClienteDetailComponent implements OnInit {
  item?: ClienteDTO;
  loading = false;
  error: string | null = null;

  constructor(
    private clienteService: ClienteService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) this.loadItem(id);
    });
  }

  loadItem(id: number): void {
    this.loading = true;
    this.error = null;

    this.clienteService.getById(id).subscribe({
      next: item => {
        this.item = item;
        this.loading = false;
      },
      error: err => {
        this.error = err.message || 'Error al cargar el cliente';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/nomencladores/clientes']);
  }
}
