import { Component, EventEmitter, Input, Output, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-modal',
    template: `
    <div class="modal-backdrop" (click)="onBackdropClick()"></div>
    <div class="modal-window">
      <div class="modal-header">
        <h3>{{ title }}</h3>
        <button class="close" (click)="close()">✖</button>
      </div>
      <div class="modal-body">
        <ng-content></ng-content>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" (click)="close()">{{ cancelText }}</button>
        <button class="btn btn-primary" (click)="confirm()">{{ confirmText }}</button>
      </div>
    </div>
  `,
    styles: [
        `.modal-backdrop { position: fixed; inset: 0; background: rgba(0,0,0,0.4); z-index: 1040 }
     .modal-window { position: fixed; left: 50%; top: 50%; transform: translate(-50%,-50%); background: white; border-radius: 8px; width: 720px; max-width: 95%; box-shadow: 0 8px 24px rgba(0,0,0,0.2); z-index: 1050; display: flex; flex-direction: column; max-height: 90vh; overflow: hidden }
     .modal-header { display:flex; justify-content:space-between; align-items:center; padding:12px 16px; border-bottom:1px solid #eee; flex: 0 0 auto }
     .modal-body { padding:16px; overflow: auto; flex: 1 1 auto }
     .modal-footer { padding:12px 16px; display:flex; justify-content:flex-end; gap:8px; border-top:1px solid #eee; flex: 0 0 auto }
     .close { background:transparent; border:none; font-size:16px }
     .btn { padding:8px 12px; border-radius:6px }
     .btn-primary { background:#0d6efd; color:white; border:none }
     .btn-secondary { background:#e9ecef; border:none }
    `
    ],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ModalComponent {
  @Input() title: string = '';
  @Input() confirmText: string = 'Guardar';
  @Input() cancelText: string = 'Cancelar';
  @Output() closed = new EventEmitter<void>();
  @Output() confirmed = new EventEmitter<void>();
  @Output() confirmRequested = new EventEmitter<void>();

  close() { this.closed.emit(); }
  confirm() { this.confirmed.emit(); }
  requestConfirm() { this.confirmRequested.emit(); }
  onBackdropClick() { this.close(); }
}
