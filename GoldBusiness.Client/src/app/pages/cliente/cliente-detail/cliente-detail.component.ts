import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Cliente, ClienteService } from '../../../services/cliente.service';

@Component({
  selector: 'app-cliente-detail',
  templateUrl: './cliente-detail.component.html',
  styleUrl: './cliente-detail.component.css'
})
export class ClienteDetailComponent {
  item?: Cliente;
  loading = false;
  error: string | null = null;

  constructor(
    private clienteService: ClienteService,
    private route: ActivatedRoute,
    private location: Location
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
      next: (item) => {
        this.item = item;
        this.loading = false;
      },
      error: (error) => {
        this.error = error.message || 'Error al cargar el cliente';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

}
