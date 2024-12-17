import { Component, inject, OnInit } from '@angular/core';
import { NotificationsService } from '../../core/services/notifications.service';
import { SnackBarService } from '../../core/services/snack-bar.service';
import { Notification } from '../../core/models/Notification';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-notifications',
  imports: [
    DatePipe,
    CommonModule
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.scss'
})
export class NotificationsComponent implements OnInit {

  notifications: Notification[]=[];
  notificationService= inject(NotificationsService);
  sanckBarService = inject(SnackBarService);

  ngOnInit(): void {
    this.loadAllNotifcations();
  }

  loadAllNotifcations() {
    this.notificationService.getAll().subscribe({
      next: (notifications: Array<any>) => {
        this.notifications = notifications;
      },
      error: (err) => {
        console.error('Error loading notifications:', err);
        this.sanckBarService.error('Failed to load notifications. Please try again.');
      }
    });
  }

  markAsRead(id: number) {
    this.notificationService.markAsRead(id).subscribe({
      next: () => {
        this.notifications = this.notifications.map(notification =>
          notification.NotificationId === id 
            ? { ...notification, IsRead: true }
            : notification
        );
        this.sanckBarService.success('Notification marked as read.');
      },
      error: (err) => {
        console.error('Error marking notification as read:', err);
        this.sanckBarService.error('Failed to mark notification as read. Please try again.');
      }
    });
  }
  

}
