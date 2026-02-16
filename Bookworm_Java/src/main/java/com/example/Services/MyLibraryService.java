
package com.example.Services;
import java.util.List;

import com.example.dto.MyLibraryItemResponseDto;

//package com.example.Services;
//
//import com.example.models.MyLibrary;
//
//import java.util.List;
//
//public interface MyLibraryService {
//
//    MyLibrary subscribeUser(MyLibrary myLibrary);
//
//    MyLibrary getActiveSubscription(Integer userId);
//
//    List<MyLibrary> getUserLibrary(Integer userId);
//
//    void incrementBooksTaken(Integer myLibId);
//
//    void decrementBooksTaken(Integer myLibId);
//}
public interface MyLibraryService {

    List<MyLibraryItemResponseDto> getUserLibrary(Integer userId);

    byte[] readLibraryBook(Integer userId, Integer productId);
}
