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
    console.log(this.notifications)
  }

  loadAllNotifcations() {
    this.notificationService.getAll().subscribe({
      next: (data: Array<any>) => {
        console.log('Notifications:', data);
        this.notifications = data.map(n => ({
          notificationId: n.notificationId,
          message: n.message,
          isRead:n.isRead,
          createdAt:n.createdAt
        }));
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
          notification.notificationId === id 
            ? { ...notification, IsRead: true }
            : notification
        );
        this.sanckBarService.success('Notification marked as read.');
        this.loadAllNotifcations();
      },
      error: (err) => {
        console.error('Error marking notification as read:', err);
        this.sanckBarService.error('Failed to mark notification as read. Please try again.');
      }
    });
  }

  delete(id:number){
    this.notificationService.delete(id).subscribe({
      next: (response) => {
        if (response) {
          this.sanckBarService.success('Notification deleted successfully.');
          window.location.reload();
        } else {
          this.sanckBarService.error('Failed to delete the notiification');
        }
      },
      error: (err) => {
        console.error('Error deleting notification :', err);
        this.sanckBarService.error('Failed to mark notification as read. Please try again.');
      }
    });
  }
  

}
